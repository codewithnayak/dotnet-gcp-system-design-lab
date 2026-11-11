
resource "google_project_service" "services" {
  for_each = toset([
    "artifactregistry.googleapis.com",
    "run.googleapis.com",
    "iam.googleapis.com"
  ])

  service            = each.value
  disable_on_destroy = false
}
