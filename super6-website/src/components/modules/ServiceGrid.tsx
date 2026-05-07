/* ─── Impact Strip — Editorial stats bar
   Two tiers:
     1. Hero stats — large serif numbers + diamond mark
     2. Credentials — single tracked-caps line of proof points
   All figures sourced from the V2 Final Investor Deck and
   site data (locations, registrationTiers, AdvantagesSection). */

import Image from "next/image";

const heroStats = [
  { number: "2014", suffix: "", label: "Founded" },
  { number: "24k", suffix: "", label: "Athletes Served Annually" },
  { number: "125k", suffix: "", label: "Annual Event Attendees" },
  { number: "2,000", suffix: "+", label: "Annual Teams" },
  { number: "300", suffix: "+", label: "Annual Clubs" },
];

const credentials = [
  "NFHS-Certified Officials",
  "Wells Fargo Financial Literacy Partner",
  "Live on HBCU League Pass",
  "Patrick Space Force Base Federal Contract",
];

export default function ImpactStrip() {
  return (
    <section className="impact-strip" aria-label="By the numbers">
      <div className="container-xl">
        <p className="impact-eyebrow">By the Numbers</p>

        {/* Tier 1 — Hero stats with diamond mark */}
        <div className="impact-row">
          <div className="impact-logo-cell">
            <Image
              src="/media/logos/super6-diamond-only.png"
              alt="Super6"
              width={160}
              height={160}
              className="impact-logo"
              priority
            />
          </div>
          <div className="impact-grid">
            {heroStats.map((stat) => (
              <div key={stat.label} className="impact-item">
                <div className="impact-number">
                  {stat.number}
                  <span className="accent">{stat.suffix}</span>
                </div>
                <div className="impact-label">{stat.label}</div>
              </div>
            ))}
          </div>
        </div>

        <hr className="impact-divider" />

        {/* Tier 2 — Credentials strip */}
        <ul className="credentials-strip" aria-label="Credentials">
          {credentials.map((c) => (
            <li key={c}>{c}</li>
          ))}
        </ul>
      </div>
    </section>
  );
}
