
resource "google_artifact_registry_repository" "catalog" {
  location      = var.region
  repository_id = "catalog-images"
  format        = "DOCKER"
  description   = "Catalog API container images"

  depends_on = [
    google_project_service.services
  ]
}
