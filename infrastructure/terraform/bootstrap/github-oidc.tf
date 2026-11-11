
variable "github_owner" {
  type = string
}

variable "github_repository" {
  type = string
}

resource "google_project_service" "sts_api" {
  service            = "sts.googleapis.com"
  disable_on_destroy = false
}

resource "google_project_service" "iam_credentials_api" {
  service            = "iamcredentials.googleapis.com"
  disable_on_destroy = false
}

resource "google_iam_workload_identity_pool" "github" {
  workload_identity_pool_id = "github-actions"
  display_name              = "GitHub Actions"

  depends_on = [
    google_project_service.sts_api,
    google_project_service.iam_credentials_api
  ]
}

resource "google_iam_workload_identity_pool_provider" "github" {
  workload_identity_pool_id          = google_iam_workload_identity_pool.github.workload_identity_pool_id
  workload_identity_pool_provider_id = "github-oidc"

  display_name = "GitHub OIDC"

  attribute_mapping = {
    "google.subject"       = "assertion.sub"
    "attribute.repository" = "assertion.repository"
    "attribute.ref"        = "assertion.ref"
  }

  attribute_condition = "assertion.repository == '${var.github_owner}/${var.github_repository}'"

  oidc {
    issuer_uri = "https://token.actions.githubusercontent.com"
  }
}

resource "google_service_account" "github_deployer" {
  account_id   = "github-deployer"
  display_name = "GitHub Actions deployment identity"
}

resource "google_service_account_iam_member" "github_wif" {
  service_account_id = google_service_account.github_deployer.name
  role               = "roles/iam.workloadIdentityUser"

  member = "principalSet://iam.googleapis.com/${google_iam_workload_identity_pool.github.name}/attribute.repository/${var.github_owner}/${var.github_repository}"
}

resource "google_artifact_registry_repository_iam_member" "github_writer" {
  project    = var.project_id
  location   = google_artifact_registry_repository.catalog.location
  repository = google_artifact_registry_repository.catalog.name

  role   = "roles/artifactregistry.writer"
  member = "serviceAccount:${google_service_account.github_deployer.email}"
}

output "github_wif_provider" {
  value = google_iam_workload_identity_pool_provider.github.name
}

output "github_deployer_email" {
  value = google_service_account.github_deployer.email
}
