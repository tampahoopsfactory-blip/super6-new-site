import type { Metadata } from "next";
import Link from "next/link";

export const metadata: Metadata = {
  title: "Sponsors | Super6 Basketball",
  description:
    "Partner with Super6 — the #1 youth basketball tournament organization in Florida. Reach coaches, players, and families across the Southeast.",
};

const tiers = [
  {
    name: "Title Partner",
    price: "Contact for pricing",
    color: "var(--s6-orange)",
    perks: [
      "Naming rights for one event (e.g. 'The [Brand] Super6 Spring Kickoff')",
      "Logo on all event signage, banners, and digital assets",
      "Top placement on website, emails, and social media",
      "Branded court presence at your named event",
      "Speaking opportunity at opening ceremony",
      "10 team registration credits",
      "Dedicated sponsor spotlight in all event communications",
    ],
  },
  {
    name: "Gold Sponsor",
    price: "Contact for pricing",
    color: "#C9A84C",
    perks: [
      "Logo on event banners at all 10 events",
      "Website sponsor page listing with logo and link",
      "Social media recognition at each event",
      "Branded presence at 3 events of your choice",
      "5 team registration credits",
      "Email newsletter placement (quarterly)",
    ],
  },
  {
    name: "Silver Sponsor",
    price: "Contact for pricing",
    color: "#8A8A8A",
    perks: [
      "Logo on website sponsor page",
      "Social media shoutout at 2 events",
      "Banner placement at 1 event of your choice",
      "2 team registration credits",
    ],
  },
  {
    name: "Supporting Partner",
    price: "Contact for pricing",
    color: "var(--s6-black)",
    perks: [
      "Logo on website sponsor page",
      "Social media recognition at season launch",
      "1 team registration credit",
    ],
  },
];

const stats = [
  { value: "1,500+", label: "Coaches in our network" },
  { value: "165+", label: "Organizations registered" },
  { value: "10", label: "Events per season" },
  { value: "4", label: "Markets (FL + Atlanta)" },
];

export default function SponsorsPage() {
  return (
    <main>
      {/* Hero */}
      <section
        style={{
          background: "var(--s6-black)",
          padding: "7rem 0 5rem",
        }}
      >
        <div className="container-xl" style={{ maxWidth: "760px" }}>
          <p
            className="editorial-eyebrow"
            style={{ color: "var(--s6-orange)" }}
          >
            Sponsorship
          </p>
          <h1 style={{ color: "var(--cream)" }}>
            Get in front of
            <br />
            <em>every serious program.</em>
          </h1>
          <p
            style={{
              color: "var(--cream)",
              opacity: 0.7,
              fontSize: "1.1rem",
              maxWidth: "540px",
              marginTop: "1.25rem",
            }}
          >
            Super6 connects you directly with coaches, players, and families
            across Florida and the Southeast — the decision-makers in youth
            basketball.
          </p>
        </div>
      </section>

      {/* Stats bar */}
      <section
        style={{
          background: "var(--s6-orange)",
          padding: "2.5rem 0",
        }}
      >
        <div className="container-xl">
          <div
            style={{
              display: "flex",
              gap: "3rem",
              justifyContent: "center",
              flexWrap: "wrap",
              textAlign: "center",
            }}
          >
            {stats.map((s) => (
              <div key={s.label}>
                <div
                  style={{
                    fontSize: "2.25rem",
                    fontWeight: 700,
                    color: "var(--s6-black)",
                    lineHeight: 1,
                  }}
                >
                  {s.value}
                </div>
                <div
                  style={{
                    fontSize: "0.8rem",
                    fontWeight: 600,
                    letterSpacing: "0.1em",
                    textTransform: "uppercase",
                    marginTop: "0.35rem",
                    color: "var(--s6-black)",
                    opacity: 0.7,
                  }}
                >
                  {s.label}
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Sponsorship tiers */}
      <section style={{ padding: "5rem 0", background: "var(--cream)" }}>
        <div className="container-xl" style={{ maxWidth: "900px" }}>
          <p
            className="editorial-eyebrow"
            style={{ color: "var(--s6-orange)", marginBottom: "0.5rem" }}
          >
            Partnership Tiers
          </p>
          <h2 style={{ marginBottom: "3rem" }}>
            Choose your <em>level of presence.</em>
          </h2>

          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(auto-fit, minmax(380px, 1fr))",
              gap: "1.5rem",
            }}
          >
            {tiers.map((tier, i) => (
              <div
                key={i}
                style={{
                  border: "1px solid var(--hairline-soft)",
                  borderRadius: "6px",
                  overflow: "hidden",
                }}
              >
                <div
                  style={{
                    padding: "1.25rem 1.5rem",
                    background: tier.color,
                  }}
                >
                  <div
                    style={{
                      fontWeight: 700,
                      fontSize: "1rem",
                      color:
                        tier.color === "var(--s6-black)"
                          ? "var(--cream)"
                          : "var(--s6-black)",
                      letterSpacing: "0.04em",
                    }}
                  >
                    {tier.name}
                  </div>
                </div>
                <div style={{ padding: "1.5rem" }}>
                  <ul style={{ margin: 0, padding: 0, listStyle: "none" }}>
                    {tier.perks.map((perk, pi) => (
                      <li
                        key={pi}
                        style={{
                          display: "flex",
                          gap: "0.65rem",
                          alignItems: "flex-start",
                          padding: "0.5rem 0",
                          borderBottom:
                            pi < tier.perks.length - 1
                              ? "1px solid var(--hairline-soft)"
                              : "none",
                          fontSize: "0.9rem",
                          color: "var(--text-muted)",
                          lineHeight: 1.5,
                        }}
                      >
                        <span
                          style={{
                            color: "var(--s6-orange)",
                            fontWeight: 700,
                            flexShrink: 0,
                            marginTop: "0.1rem",
                          }}
                        >
                          ✓
                        </span>
                        {perk}
                      </li>
                    ))}
                  </ul>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CTA */}
      <section
        style={{
          background: "var(--s6-black)",
          padding: "5rem 0",
          textAlign: "center",
        }}
      >
        <div className="container-xl" style={{ maxWidth: "560px" }}>
          <h2 style={{ color: "var(--cream)", marginBottom: "1rem" }}>
            Ready to partner with Super6?
          </h2>
          <p
            style={{
              color: "var(--cream)",
              opacity: 0.7,
              fontSize: "1.05rem",
              marginBottom: "2rem",
            }}
          >
            We work with brands that align with competitive youth sports. Reach
            out and let us build the right package for you.
          </p>
          <Link
            href="/contact"
            className="btn-primary"
            style={{ fontSize: "1rem", padding: "0.9rem 2rem" }}
          >
            Get Sponsorship Info
          </Link>
        </div>
      </section>
    </main>
  );
}
