import Image from "next/image";
import Link from "next/link";
import { locations } from "@/data/site";

/* ─── Locations Grid — Cinematic city grid
   Nike: full-bleed photography, bold city names, minimal chrome.
   Asymmetric grid: 3 top, 2 bottom (one wider). ─── */
export default function LocationsGrid() {
  return (
    <section className="section" style={{ background: "var(--black)", padding: "0" }} aria-label="Locations">
      <div style={{ padding: "var(--space-10) 0 0" }}>
        <div className="container-xl" style={{ textAlign: "center", marginBottom: "48px" }}>
          <p className="section-label">Our Markets</p>
          <h2 className="section-heading">
            Five Cities. One Standard.
          </h2>
        </div>
      </div>

      <div className="locations-grid">
        {locations.map((loc) => (
          <Link key={loc.slug} href={`/locations/${loc.slug}`} className="location-card">
            <Image
              src={loc.image}
              alt={`${loc.city}, ${loc.state}`}
              fill
              sizes="(max-width: 768px) 100vw, 33vw"
              className="object-cover"
            />
            <div className="location-card-overlay" />
            <div className="location-card-content">
              <h3 className="location-city">{loc.city}</h3>
              <p className="location-state">{loc.state}</p>
            </div>
            {loc.comingSoon && (
              <span className="location-badge">Coming Soon</span>
            )}
          </Link>
        ))}
      </div>
    </section>
  );
}
