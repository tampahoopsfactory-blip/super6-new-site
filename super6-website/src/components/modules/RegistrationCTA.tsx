import Link from "next/link";
import { registrationTiers } from "@/data/site";

/* ─── Pricing — Editorial registration tiers
   Three cream cards on paper background, single featured.
   Serif prices, hairline dividers between features. */

export default function PricingSection() {
  return (
    <section className="section section-paper" aria-labelledby="pricing-heading">
      <div className="container-xl">
        <div style={{ textAlign: "center", marginBottom: 64, display: "flex", flexDirection: "column", alignItems: "center" }}>
          <p className="section-label">Registration</p>
          <h2 id="pricing-heading" className="section-heading" style={{ marginBottom: 16 }}>
            Pick your <em>weekend.</em>
          </h2>
          <p className="section-desc" style={{ textAlign: "center" }}>
            Boys 12th–3rd grade and Girls 12th–6th grade across three
            divisions. One tournament, a full season, or a club package.
          </p>
        </div>

        <div className="pricing-grid">
          {registrationTiers.map((tier) => (
            <div
              key={tier.id}
              className={`pricing-card ${tier.popular ? "pricing-card--featured" : ""}`}
            >
              {tier.popular && <span className="pricing-badge">Most Popular</span>}
              <p className="pricing-name">{tier.name}</p>
              <p className="pricing-price">{tier.priceLabel}</p>
              <p className="pricing-period">{tier.period}</p>
              <ul className="pricing-features">
                {tier.features.map((feature) => (
                  <li key={feature}>{feature}</li>
                ))}
              </ul>
              <Link
                href={tier.id === "club-package" ? "/contact" : "/register"}
                className={`btn ${tier.popular ? "btn-orange" : "btn-outline"}`}
                style={{ width: "100%" }}
              >
                {tier.cta}
              </Link>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
