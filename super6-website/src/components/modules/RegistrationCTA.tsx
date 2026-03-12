import Link from "next/link";
import { registrationTiers } from "@/data/site";

/* ─── Pricing — Clean corporate tiers
   Anthropic: generous whitespace, subtle borders, clear hierarchy.
   Featured card has bold border, badge, orange CTA. ─── */
export default function PricingSection() {
  return (
    <section className="section" aria-labelledby="pricing-heading">
      <div className="container-xl">
        <div style={{ textAlign: "center", marginBottom: "56px" }}>
          <p className="section-label">Registration</p>
          <h2 id="pricing-heading" className="section-heading" style={{ margin: "0 auto 16px" }}>
            Register Your Team
          </h2>
          <p className="section-desc" style={{ margin: "0 auto" }}>
            Boys 12th–3rd grade and Girls 12th–6th grade. Elite, Competitive,
            and Developmental divisions at every tournament.
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
                className={`btn ${tier.popular ? "btn-black" : "btn-outline"}`}
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
