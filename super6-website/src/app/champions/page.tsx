import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";

export const metadata: Metadata = {
  title: "Champions | Super6 Basketball",
  description:
    "Super6 tournament champions — the programs that competed, won, and came back stronger every season.",
};

const seasons = [
  {
    year: "2025",
    events: [
      {
        name: "Spring Kickoff · Orlando",
        champions: [
          { division: "3rd–5th", team: "Orlando Elite Gold" },
          { division: "6th–8th", team: "Central Florida Hoops" },
          { division: "9th–12th", team: "Next Level Athletics" },
        ],
      },
      {
        name: "Summer Opener · Tampa",
        champions: [
          { division: "3rd–5th", team: "Tampa Bay Ballers" },
          { division: "6th–8th", team: "Sunshine State Select" },
          { division: "9th–12th", team: "Florida Finest" },
        ],
      },
      {
        name: "Gulf Coast Classic · Clearwater",
        champions: [
          { division: "3rd–5th", team: "Gulf Coast Gators" },
          { division: "6th–8th", team: "West Coast Warriors" },
          { division: "9th–12th", team: "Pinellas Prodigies" },
        ],
      },
      {
        name: "Season Finals · Orlando",
        champions: [
          { division: "3rd–5th", team: "Orlando Elite Gold" },
          { division: "6th–8th", team: "Next Level Athletics" },
          { division: "9th–12th", team: "Central Florida Hoops" },
        ],
      },
    ],
  },
  {
    year: "2024",
    events: [
      {
        name: "Spring Kickoff · Orlando",
        champions: [
          { division: "3rd–5th", team: "Florida Storm Gold" },
          { division: "6th–8th", team: "Orange County Elite" },
          { division: "9th–12th", team: "The Program FL" },
        ],
      },
      {
        name: "Season Finals · Orlando",
        champions: [
          { division: "3rd–5th", team: "Central Florida Ballers" },
          { division: "6th–8th", team: "Florida Storm Gold" },
          { division: "9th–12th", team: "OC Elite Blue" },
        ],
      },
    ],
  },
];

export default function ChampionsPage() {
  return (
    <main>
      {/* Hero */}
      <section className="page-hero">
        <Image
          src="/media/curated/05-trophy-presentation.jpg"
          alt=""
          fill
          priority
          quality={92}
          sizes="100vw"
          aria-hidden="true"
        />
        <div className="container-xl">
          <p
            className="editorial-eyebrow"
            style={{ color: "var(--cream)", opacity: 0.85 }}
          >
            Super6 · Champions
          </p>
          <h1>
            The programs
            <br />
            <em>that won.</em>
          </h1>
          <p
            style={{
              color: "var(--cream)",
              fontSize: "1.1rem",
              maxWidth: "500px",
              marginTop: "1.25rem",
            }}
          >
            Division champions from every Super6 event on record.
          </p>
        </div>
      </section>

      {/* Champions by season */}
      <section style={{ padding: "5rem 0", background: "var(--cream)" }}>
        <div className="container-xl" style={{ maxWidth: "900px" }}>
          {seasons.map((season, si) => (
            <div
              key={si}
              style={{ marginBottom: si < seasons.length - 1 ? "5rem" : 0 }}
            >
              <div
                style={{
                  display: "flex",
                  alignItems: "baseline",
                  gap: "1rem",
                  marginBottom: "2.5rem",
                  borderBottom: "2px solid var(--s6-black)",
                  paddingBottom: "0.75rem",
                }}
              >
                <h2 style={{ fontSize: "2rem", margin: 0 }}>{season.year}</h2>
                <span
                  style={{
                    fontSize: "0.8rem",
                    fontWeight: 700,
                    letterSpacing: "0.12em",
                    textTransform: "uppercase",
                    color: "var(--s6-orange)",
                  }}
                >
                  Season
                </span>
              </div>

              <div
                style={{
                  display: "flex",
                  flexDirection: "column",
                  gap: "2.5rem",
                }}
              >
                {season.events.map((event, ei) => (
                  <div key={ei}>
                    <p
                      className="editorial-eyebrow"
                      style={{
                        color: "var(--text-muted)",
                        marginBottom: "1rem",
                      }}
                    >
                      {event.name}
                    </p>
                    <div
                      style={{
                        display: "grid",
                        gridTemplateColumns:
                          "repeat(auto-fit, minmax(220px, 1fr))",
                        gap: "1rem",
                      }}
                    >
                      {event.champions.map((champ, ci) => (
                        <div
                          key={ci}
                          style={{
                            padding: "1.25rem",
                            background: "var(--s6-black)",
                            borderRadius: "4px",
                          }}
                        >
                          <div
                            style={{
                              fontSize: "0.68rem",
                              fontWeight: 700,
                              letterSpacing: "0.12em",
                              textTransform: "uppercase",
                              color: "var(--s6-orange)",
                              marginBottom: "0.4rem",
                            }}
                          >
                            {champ.division}
                          </div>
                          <div
                            style={{
                              fontWeight: 700,
                              fontSize: "0.95rem",
                              color: "var(--cream)",
                            }}
                          >
                            {champ.team}
                          </div>
                        </div>
                      ))}
                    </div>
                  </div>
                ))}
              </div>
            </div>
          ))}
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
            Add your name to the list.
          </h2>
          <p
            style={{
              color: "var(--cream)",
              opacity: 0.7,
              fontSize: "1.05rem",
              marginBottom: "2rem",
            }}
          >
            The 2026 season is live. Register your team and compete for a
            championship every event.
          </p>
          <Link
            href="/register"
            className="btn-primary"
            style={{ fontSize: "1rem", padding: "0.9rem 2rem" }}
          >
            Register Your Team
          </Link>
        </div>
      </section>
    </main>
  );
}
