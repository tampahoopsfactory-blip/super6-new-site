"""
EventShield Pro - White-Label Branding Service
Dynamic branding and tenant management for commercial licensing
"""

import os
import json
import base64
from typing import Dict, Any, Optional, List
from dataclasses import dataclass, asdict
from datetime import datetime
import logging
from pathlib import Path
import hashlib

from app.core.logging import logger
from app.core.config import get_config


@dataclass
class BrandingConfig:
    """Client branding configuration"""
    tenant_id: str
    company_name: str
    logo_url: Optional[str] = None
    logo_data: Optional[str] = None  # Base64 encoded logo
    primary_color: str = "#007AFF"  # Apple blue default
    secondary_color: str = "#0056CC"
    accent_color: str = "#34C759"
    background_color: str = "#FFFFFF"
    text_color: str = "#1D1D1F"
    font_family: str = "SF Pro Display, -apple-system, BlinkMacSystemFont, sans-serif"
    custom_css: Optional[str] = None
    favicon_url: Optional[str] = None
    favicon_data: Optional[str] = None  # Base64 encoded favicon
    custom_domain: Optional[str] = None
    theme_mode: str = "light"  # light, dark, auto
    created_at: datetime = None
    updated_at: datetime = None
    
    def __post_init__(self):
        if self.created_at is None:
            self.created_at = datetime.utcnow()
        if self.updated_at is None:
            self.updated_at = datetime.utcnow()


@dataclass
class TenantInfo:
    """Tenant information"""
    tenant_id: str
    name: str
    domain: str
    license_key: str
    features: List[str]
    branding: BrandingConfig
    settings: Dict[str, Any]
    created_at: datetime
    updated_at: datetime
    is_active: bool = True


class BrandingService:
    """White-label branding service for EventShield Pro"""
    
    def __init__(self):
        self.config = get_config()
        self.tenants: Dict[str, TenantInfo] = {}
        self.branding_cache: Dict[str, BrandingConfig] = {}
        self.assets_dir = Path("assets/branding")
        self.assets_dir.mkdir(parents=True, exist_ok=True)
        
        # Load existing tenants
        self._load_tenants()
    
    def create_tenant(self, tenant_data: Dict[str, Any]) -> TenantInfo:
        """Create new tenant with branding"""
        try:
            tenant_id = tenant_data.get("tenant_id")
            if not tenant_id:
                tenant_id = self._generate_tenant_id(tenant_data["name"])
            
            # Create branding configuration
            branding = BrandingConfig(
                tenant_id=tenant_id,
                company_name=tenant_data["name"],
                primary_color=tenant_data.get("primary_color", "#007AFF"),
                secondary_color=tenant_data.get("secondary_color", "#0056CC"),
                accent_color=tenant_data.get("accent_color", "#34C759"),
                background_color=tenant_data.get("background_color", "#FFFFFF"),
                text_color=tenant_data.get("text_color", "#1D1D1F"),
                font_family=tenant_data.get("font_family", "SF Pro Display, -apple-system, BlinkMacSystemFont, sans-serif"),
                custom_css=tenant_data.get("custom_css"),
                theme_mode=tenant_data.get("theme_mode", "light")
            )
            
            # Handle logo upload
            if "logo" in tenant_data:
                branding.logo_data = self._process_logo(tenant_data["logo"], tenant_id)
            
            # Handle favicon upload
            if "favicon" in tenant_data:
                branding.favicon_data = self._process_favicon(tenant_data["favicon"], tenant_id)
            
            # Create tenant
            tenant = TenantInfo(
                tenant_id=tenant_id,
                name=tenant_data["name"],
                domain=tenant_data.get("domain", f"{tenant_id}.eventshield.com"),
                license_key=self._generate_license_key(tenant_id),
                features=tenant_data.get("features", []),
                branding=branding,
                settings=tenant_data.get("settings", {}),
                created_at=datetime.utcnow(),
                updated_at=datetime.utcnow()
            )
            
            # Store tenant
            self.tenants[tenant_id] = tenant
            self.branding_cache[tenant_id] = branding
            
            # Save to storage
            self._save_tenant(tenant)
            
            logger.logger.info(
                "Tenant Created",
                tenant_id=tenant_id,
                company_name=tenant.name,
                domain=tenant.domain
            )
            
            return tenant
            
        except Exception as e:
            logger.logger.error(
                "Failed to create tenant",
                error=str(e),
                tenant_data=tenant_data
            )
            raise
    
    def update_tenant_branding(self, tenant_id: str, branding_data: Dict[str, Any]) -> BrandingConfig:
        """Update tenant branding configuration"""
        try:
            if tenant_id not in self.tenants:
                raise ValueError(f"Tenant {tenant_id} not found")
            
            tenant = self.tenants[tenant_id]
            branding = tenant.branding
            
            # Update branding fields
            for key, value in branding_data.items():
                if hasattr(branding, key):
                    setattr(branding, key, value)
            
            # Handle logo update
            if "logo" in branding_data:
                branding.logo_data = self._process_logo(branding_data["logo"], tenant_id)
            
            # Handle favicon update
            if "favicon" in branding_data:
                branding.favicon_data = self._process_favicon(branding_data["favicon"], tenant_id)
            
            branding.updated_at = datetime.utcnow()
            
            # Update cache
            self.branding_cache[tenant_id] = branding
            
            # Save to storage
            self._save_tenant(tenant)
            
            logger.logger.info(
                "Tenant Branding Updated",
                tenant_id=tenant_id,
                company_name=tenant.name
            )
            
            return branding
            
        except Exception as e:
            logger.logger.error(
                "Failed to update tenant branding",
                tenant_id=tenant_id,
                error=str(e)
            )
            raise
    
    def get_tenant_branding(self, tenant_id: str) -> Optional[BrandingConfig]:
        """Get tenant branding configuration"""
        if tenant_id in self.branding_cache:
            return self.branding_cache[tenant_id]
        
        # Load from storage if not in cache
        tenant = self._load_tenant(tenant_id)
        if tenant:
            self.branding_cache[tenant_id] = tenant.branding
            return tenant.branding
        
        return None
    
    def get_tenant_by_domain(self, domain: str) -> Optional[TenantInfo]:
        """Get tenant by domain"""
        for tenant in self.tenants.values():
            if tenant.domain == domain:
                return tenant
        return None
    
    def generate_branding_css(self, tenant_id: str) -> str:
        """Generate CSS for tenant branding"""
        branding = self.get_tenant_branding(tenant_id)
        if not branding:
            return ""
        
        css = f"""
/* EventShield Pro - {branding.company_name} Branding */
:root {{
    --primary-color: {branding.primary_color};
    --secondary-color: {branding.secondary_color};
    --accent-color: {branding.accent_color};
    --background-color: {branding.background_color};
    --text-color: {branding.text_color};
    --font-family: {branding.font_family};
}}

/* Override default colors */
.MuiButton-contained {{
    background-color: var(--primary-color) !important;
}}

.MuiButton-contained:hover {{
    background-color: var(--secondary-color) !important;
}}

.MuiChip-colorPrimary {{
    background-color: var(--primary-color) !important;
}}

.MuiSwitch-colorPrimary .MuiSwitch-switchBase.Mui-checked {{
    color: var(--primary-color) !important;
}}

.MuiSwitch-colorPrimary .MuiSwitch-switchBase.Mui-checked + .MuiSwitch-track {{
    background-color: var(--primary-color) !important;
}}

/* Custom branding styles */
.branded-header {{
    background: linear-gradient(135deg, var(--primary-color), var(--secondary-color));
    color: white;
}}

.branded-card {{
    border: 1px solid var(--primary-color);
    box-shadow: 0 2px 8px rgba(0, 122, 255, 0.1);
}}

.branded-text {{
    color: var(--text-color);
    font-family: var(--font-family);
}}

/* Custom CSS from tenant */
{branding.custom_css or ''}
"""
        
        return css
    
    def generate_branding_js(self, tenant_id: str) -> str:
        """Generate JavaScript for tenant branding"""
        branding = self.get_tenant_branding(tenant_id)
        if not branding:
            return ""
        
        js = f"""
// EventShield Pro - {branding.company_name} Branding
window.EventShieldBranding = {{
    tenantId: '{tenant_id}',
    companyName: '{branding.company_name}',
    primaryColor: '{branding.primary_color}',
    secondaryColor: '{branding.secondary_color}',
    accentColor: '{branding.accent_color}',
    backgroundColor: '{branding.background_color}',
    textColor: '{branding.text_color}',
    fontFamily: '{branding.font_family}',
    themeMode: '{branding.theme_mode}',
    
    // Apply branding on page load
    applyBranding: function() {{
        // Update page title
        document.title = '{branding.company_name} - EventShield Pro';
        
        // Update favicon if available
        if ('{branding.favicon_data}') {{
            const favicon = document.querySelector('link[rel="icon"]') || document.createElement('link');
            favicon.rel = 'icon';
            favicon.href = 'data:image/png;base64,{branding.favicon_data}';
            document.head.appendChild(favicon);
        }}
        
        // Apply theme mode
        if ('{branding.theme_mode}' === 'dark') {{
            document.body.classList.add('dark-theme');
        }}
        
        // Update CSS custom properties
        const root = document.documentElement;
        root.style.setProperty('--primary-color', '{branding.primary_color}');
        root.style.setProperty('--secondary-color', '{branding.secondary_color}');
        root.style.setProperty('--accent-color', '{branding.accent_color}');
        root.style.setProperty('--background-color', '{branding.background_color}');
        root.style.setProperty('--text-color', '{branding.text_color}');
        root.style.setProperty('--font-family', '{branding.font_family}');
    }}
}};

// Apply branding when DOM is ready
if (document.readyState === 'loading') {{
    document.addEventListener('DOMContentLoaded', window.EventShieldBranding.applyBranding);
}} else {{
    window.EventShieldBranding.applyBranding();
}}
"""
        
        return js
    
    def get_tenant_logo(self, tenant_id: str) -> Optional[str]:
        """Get tenant logo as base64 data URL"""
        branding = self.get_tenant_branding(tenant_id)
        if not branding or not branding.logo_data:
            return None
        
        return f"data:image/png;base64,{branding.logo_data}"
    
    def get_tenant_favicon(self, tenant_id: str) -> Optional[str]:
        """Get tenant favicon as base64 data URL"""
        branding = self.get_tenant_branding(tenant_id)
        if not branding or not branding.favicon_data:
            return None
        
        return f"data:image/x-icon;base64,{branding.favicon_data}"
    
    def validate_branding_data(self, branding_data: Dict[str, Any]) -> List[str]:
        """Validate branding data and return errors"""
        errors = []
        
        # Validate colors
        color_fields = ["primary_color", "secondary_color", "accent_color", "background_color", "text_color"]
        for field in color_fields:
            if field in branding_data:
                if not self._is_valid_color(branding_data[field]):
                    errors.append(f"Invalid color format for {field}: {branding_data[field]}")
        
        # Validate theme mode
        if "theme_mode" in branding_data:
            if branding_data["theme_mode"] not in ["light", "dark", "auto"]:
                errors.append(f"Invalid theme mode: {branding_data['theme_mode']}")
        
        # Validate logo
        if "logo" in branding_data:
            if not self._is_valid_image_data(branding_data["logo"]):
                errors.append("Invalid logo data format")
        
        # Validate favicon
        if "favicon" in branding_data:
            if not self._is_valid_image_data(branding_data["favicon"]):
                errors.append("Invalid favicon data format")
        
        return errors
    
    def _generate_tenant_id(self, company_name: str) -> str:
        """Generate unique tenant ID from company name"""
        # Create a URL-friendly ID from company name
        base_id = company_name.lower().replace(" ", "-").replace("_", "-")
        base_id = "".join(c for c in base_id if c.isalnum() or c == "-")
        
        # Add timestamp to ensure uniqueness
        timestamp = str(int(datetime.utcnow().timestamp()))
        return f"{base_id}-{timestamp[-6:]}"
    
    def _generate_license_key(self, tenant_id: str) -> str:
        """Generate license key for tenant"""
        # Create a hash-based license key
        data = f"{tenant_id}-{datetime.utcnow().isoformat()}"
        hash_obj = hashlib.sha256(data.encode())
        return hash_obj.hexdigest()[:32].upper()
    
    def _process_logo(self, logo_data: str, tenant_id: str) -> str:
        """Process and store logo data"""
        try:
            # Validate base64 data
            if not self._is_valid_image_data(logo_data):
                raise ValueError("Invalid logo data format")
            
            # Store logo file
            logo_path = self.assets_dir / f"{tenant_id}_logo.png"
            with open(logo_path, "wb") as f:
                f.write(base64.b64decode(logo_data))
            
            # Return base64 data for storage
            return logo_data
            
        except Exception as e:
            logger.logger.error(
                "Failed to process logo",
                tenant_id=tenant_id,
                error=str(e)
            )
            raise
    
    def _process_favicon(self, favicon_data: str, tenant_id: str) -> str:
        """Process and store favicon data"""
        try:
            # Validate base64 data
            if not self._is_valid_image_data(favicon_data):
                raise ValueError("Invalid favicon data format")
            
            # Store favicon file
            favicon_path = self.assets_dir / f"{tenant_id}_favicon.ico"
            with open(favicon_path, "wb") as f:
                f.write(base64.b64decode(favicon_data))
            
            # Return base64 data for storage
            return favicon_data
            
        except Exception as e:
            logger.logger.error(
                "Failed to process favicon",
                tenant_id=tenant_id,
                error=str(e)
            )
            raise
    
    def _is_valid_color(self, color: str) -> bool:
        """Validate color format (hex, rgb, rgba)"""
        import re
        
        # Hex color pattern
        hex_pattern = r'^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$'
        if re.match(hex_pattern, color):
            return True
        
        # RGB/RGBA pattern
        rgb_pattern = r'^rgba?\(\s*\d+\s*,\s*\d+\s*,\s*\d+\s*(,\s*[\d.]+)?\s*\)$'
        if re.match(rgb_pattern, color):
            return True
        
        return False
    
    def _is_valid_image_data(self, data: str) -> bool:
        """Validate base64 image data"""
        try:
            # Check if it's valid base64
            decoded = base64.b64decode(data)
            
            # Check if it's a valid image format
            if data.startswith('data:image/'):
                return True
            
            # Check magic bytes for common image formats
            image_signatures = [
                b'\xFF\xD8\xFF',  # JPEG
                b'\x89PNG\r\n\x1a\n',  # PNG
                b'BM',  # BMP
                b'GIF87a',  # GIF
                b'GIF89a',  # GIF
            ]
            
            for signature in image_signatures:
                if decoded.startswith(signature):
                    return True
            
            return False
            
        except Exception:
            return False
    
    def _load_tenants(self):
        """Load tenants from storage"""
        try:
            tenants_file = self.assets_dir / "tenants.json"
            if tenants_file.exists():
                with open(tenants_file, 'r') as f:
                    tenants_data = json.load(f)
                
                for tenant_data in tenants_data:
                    tenant = self._dict_to_tenant(tenant_data)
                    self.tenants[tenant.tenant_id] = tenant
                    self.branding_cache[tenant.tenant_id] = tenant.branding
                
                logger.logger.info(f"Loaded {len(self.tenants)} tenants from storage")
                
        except Exception as e:
            logger.logger.error(f"Failed to load tenants: {e}")
    
    def _save_tenant(self, tenant: TenantInfo):
        """Save tenant to storage"""
        try:
            tenants_file = self.assets_dir / "tenants.json"
            
            # Load existing tenants
            tenants_data = []
            if tenants_file.exists():
                with open(tenants_file, 'r') as f:
                    tenants_data = json.load(f)
            
            # Update or add tenant
            tenant_dict = self._tenant_to_dict(tenant)
            existing_index = next((i for i, t in enumerate(tenants_data) if t["tenant_id"] == tenant.tenant_id), -1)
            
            if existing_index >= 0:
                tenants_data[existing_index] = tenant_dict
            else:
                tenants_data.append(tenant_dict)
            
            # Save to file
            with open(tenants_file, 'w') as f:
                json.dump(tenants_data, f, indent=2, default=str)
            
        except Exception as e:
            logger.logger.error(f"Failed to save tenant {tenant.tenant_id}: {e}")
    
    def _load_tenant(self, tenant_id: str) -> Optional[TenantInfo]:
        """Load specific tenant from storage"""
        try:
            tenants_file = self.assets_dir / "tenants.json"
            if tenants_file.exists():
                with open(tenants_file, 'r') as f:
                    tenants_data = json.load(f)
                
                for tenant_data in tenants_data:
                    if tenant_data["tenant_id"] == tenant_id:
                        return self._dict_to_tenant(tenant_data)
            
        except Exception as e:
            logger.logger.error(f"Failed to load tenant {tenant_id}: {e}")
        
        return None
    
    def _tenant_to_dict(self, tenant: TenantInfo) -> Dict[str, Any]:
        """Convert TenantInfo to dictionary"""
        return {
            "tenant_id": tenant.tenant_id,
            "name": tenant.name,
            "domain": tenant.domain,
            "license_key": tenant.license_key,
            "features": tenant.features,
            "branding": asdict(tenant.branding),
            "settings": tenant.settings,
            "created_at": tenant.created_at.isoformat(),
            "updated_at": tenant.updated_at.isoformat(),
            "is_active": tenant.is_active
        }
    
    def _dict_to_tenant(self, data: Dict[str, Any]) -> TenantInfo:
        """Convert dictionary to TenantInfo"""
        branding_data = data["branding"]
        branding = BrandingConfig(**branding_data)
        
        return TenantInfo(
            tenant_id=data["tenant_id"],
            name=data["name"],
            domain=data["domain"],
            license_key=data["license_key"],
            features=data["features"],
            branding=branding,
            settings=data["settings"],
            created_at=datetime.fromisoformat(data["created_at"]),
            updated_at=datetime.fromisoformat(data["updated_at"]),
            is_active=data.get("is_active", True)
        )


# Global branding service instance
branding_service = BrandingService()
