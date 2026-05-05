import Image from "next/image";
import Link from "next/link";
import { locations } from "@/data/site";

/* ─── Location Sections — One photo per section.
   Each market gets its own alternating split. No 5-photo grid clustering.
   Atlanta carries a Coming Soon badge in its content panel. */

export default function LocationSections() {
  return (
    <>
      {/* Section intro */}
      <section className="section section-paper" aria-label="Our Markets">
        <div className="container-xl" style={{ textAlign: "center" }}>
          <p className="section-label" style={{ justifyContent: "center" }}>
            Our Markets
          </p>
          <h2 className="section-heading" style={{ margin: "0 auto 16px" }}>
            Five cities. <em>One standard.</em>
          </h2>
          <p className="section-desc" style={{ margin: "0 auto" }}>
            Each market gets the same level of officiating, branding, and
            production &mdash; whether it&rsquo;s a flagship Orlando weekend
            or a new launch on the road.
          </p>
        </div>
      </section>

      {/* One section per location, alternating */}
      {locations.map((loc, i) => {
        const left = i % 2 === 0;
        return (
          <section
            key={loc.slug}
            aria-label={loc.name}
            className={i % 2 === 1 ? "split-warm" : ""}
          >
            <div className="split">
              <div
                className="split-image"
                style={{ order: left ? 0 : 1 }}
              >
                <Image
                  src={loc.image}
                  alt={`${loc.city}, ${loc.state}`}
                  fill
                  sizes="(max-width: 968px) 100vw, 50vw"
                  quality={92}
                  style={{ objectFit: "cover" }}
                />
                {loc.comingSoon && (
                  <span className="location-badge" style={{ position: "absolute", top: 20, right: 20, zIndex: 3 }}>
                    Coming Soon
                  </span>
                )}
              </div>
              <div
                className="split-content"
                style={{ order: left ? 1 : 0 }}
              >
                <p className="section-label">
                  {loc.state === "GA" ? "Georgia" : "Florida"} &middot; {loc.city}
                </p>
                <h2 className="section-heading" style={{ maxWidth: "16ch" }}>
                  {loc.name.replace(/^Super6 Series LLC /, "")}
                </h2>
                <p className="section-desc" style={{ marginBottom: 36 }}>
                  {loc.description}
                </p>
                <div>
                  <Link
                    href={`/locations/${loc.slug}`}
                    className={loc.comingSoon ? "btn btn-outline" : "btn btn-ink"}
                  >
                    {loc.comingSoon ? "Get notified" : "View location"}
                  </Link>
                </div>
              </div>
            </div>
          </section>
        );
      })}
    </>
  );
}
