import Image from "next/image";
import Link from "next/link";
import { locations } from "@/data/site";

/* ─── Locations Grid — Cinematic city grid
   Editorial: section header on cream, then full-bleed city tiles below.
   Asymmetric grid: 3 across top, then 1+2 wider on the second row. */

export default function LocationsGrid() {
  return (
    <section aria-label="Locations" style={{ background: "var(--cream)" }}>
      <div style={{ padding: "var(--space-9) 0 var(--space-7)" }}>
        <div className="container-xl">
          <p className="section-label">Our Markets</p>
          <h2 className="section-heading" style={{ maxWidth: "20ch" }}>
            Five cities. <em>One standard.</em>
          </h2>
          <p className="section-desc" style={{ marginTop: 8 }}>
            Each market gets the same level of officiating, branding, and
            production — whether it’s a flagship Orlando weekend or a new
            launch on the road.
          </p>
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
              quality={88}
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
