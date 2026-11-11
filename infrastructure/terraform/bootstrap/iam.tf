
resource "google_service_account" "catalog_runtime" {
  account_id   = "catalog-runtime"
  display_name = "Catalog API Cloud Run runtime"

  depends_on = [
    google_project_service.services
  ]
}
