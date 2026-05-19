class LicenseService:
    def check_feature(self, tenant_id, feature_name):
        return True

    def get_license_status(self, tenant_id):
        return {'plan': 'standard', 'status': 'active'}

    def check_event_limit(self, tenant_id):
        return True

    def increment_event_usage(self, tenant_id):
        pass

    def decrement_event_usage(self, tenant_id):
        pass
