import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";

export const metadata: Metadata = {
  title: "2026 Schedule | Super6 Basketball",
  description:
    "Ten events across Florida and Atlanta. Register for individual tournaments or lock in the $899 season pass for all 10.",
};

const events = [
  {
    id: 1,
    month: "May",
    day: "17–18",
    name: "Spring Kickoff",
    city: "Orlando",
    state: "FL",
    divisions: ["3rd–5th", "6th–8th", "9th–12th"],
  },
  {
    id: 2,
    month: "Jun",
    day: "7–8",
    name: "Summer Opener",
    city: "Tampa",
    state: "FL",
    divisions: ["3rd–5th", "6th–8th", "9th–12th"],
  },
  {
    id: 3,
    month: "Jun",
    day: "21–22",
    name: "Gulf Coast Classic",
    city: "Clearwater",
    state: "FL",
    divisions: ["3rd–5th", "6th–8th", "9th–12th"],
  },
  {
    id: 4,
    month: "Jul",
    day: "12–13",
    name: "Mid-Summer Invitational",
    city: "Orlando",
    state: "FL",
    divisions: ["3rd–5th", "6th–8th", "9th–12th"],
  },
  {
    id: 5,
    month: "Jul",
    day: "26–27",
    name: "Palm Beach Showcase",
    city: "West Palm Beach",
    state: "FL",
    divisions: ["3rd–5th", "6th–8th", "9th–12th"],
  },
  {
    id: 6,
    month: "Aug",
    day: "9–10",
    name: "Back to School Brawl",
    city: "Tampa",
    state: "FL",
    divisions: ["3rd–5th", "6th–8th", "9th–12th"],
  },
  {
    id: 7,
    month: "Sep",
    day: "13–14",
    name: "Fall Opener",
    city: "Orlando",
    state: "FL",
    divisions: ["3rd–5th", "6th–8th", "9th–12th"],
  },
  {
    id: 8,
    month: "Oct",
    day: "4–5",
    name: "Atlanta Invitational",
    city: "Atlanta",
    state: "GA",
    divisions: ["3rd–5th", "6th–8th", "9th–12th"],
  },
  {
    id: 9,
    month: "Nov",
    day: "8–9",
    name: "Gulf Championship",
    city: "Clearwater",
    state: "FL",
    divisions: ["3rd–5th", "6th–8th", "9th–12th"],
  },
  {
    id: 10,
    month: "Dec",
    day: "6–7",
    name: "Season Finals",
    city: "Orlando",
    state: "FL",
    divisions: ["3rd–5th", "6th–8th", "9th–12th"],
  },
];

export default function SchedulePage() {
  return (
    <main>
      {/* Hero */}
      <section className="page-hero">
        <Image
          src="/media/curated/22-scorers-table.jpg"
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
            #1 Tournament Organization in Florida · 2026 Season
          </p>
          <h1>
            Ten events.
            <br />
            <em>All season.</em>
          </h1>
          <p
            style={{
              color: "var(--cream)",
              fontSize: "1.15rem",
              maxWidth: "520px",
              marginTop: "1.25rem",
            }}
          >
            May through December. Four venues across Florida and Atlanta. Lock
            in the season pass and play every event for one flat price.
          </p>
        </div>
      </section>

      {/* Facts bar */}
      <section
        style={{
          background: "var(--s6-black)",
          color: "var(--cream)",
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
            {[
              { stat: "10", label: "Events" },
              { stat: "4", label: "Venues" },
              { stat: "3+", label: "Guaranteed Games" },
              { stat: "$99", label: "Per Event" },
            ].map((item) => (
              <div key={item.label}>
                <div
                  style={{
                    fontSize: "2.5rem",
                    fontWeight: 700,
                    color: "var(--s6-orange)",
                    lineHeight: 1,
                  }}
                >
                  {item.stat}
                </div>
                <div
                  style={{
                    fontSize: "0.85rem",
                    letterSpacing: "0.1em",
                    textTransform: "uppercase",
                    marginTop: "0.4rem",
                    opacity: 0.7,
                  }}
                >
                  {item.label}
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Event list */}
      <section style={{ padding: "4rem 0", background: "var(--cream)" }}>
        <div className="container-xl" style={{ maxWidth: "860px" }}>
          <div style={{ display: "flex", flexDirection: "column", gap: "0" }}>
            {events.map((event, index) => (
              <div
                key={event.id}
                style={{
                  display: "grid",
                  gridTemplateColumns: "72px 1fr auto",
                  gap: "1.5rem",
                  alignItems: "center",
                  padding: "1.75rem 0",
                  borderBottom:
                    index < events.length - 1
                      ? "1px solid var(--hairline-soft)"
                      : "none",
                }}
              >
                {/* Date block */}
                <div style={{ textAlign: "center" }}>
                  <div
                    style={{
                      fontSize: "0.7rem",
                      fontWeight: 700,
                      textTransform: "uppercase",
                      letterSpacing: "0.12em",
                      color: "var(--text-muted)",
                    }}
                  >
                    {event.month}
                  </div>
                  <div
                    style={{
                      fontSize: "1.6rem",
                      fontWeight: 700,
                      lineHeight: 1,
                      color: "var(--s6-black)",
                    }}
                  >
                    {event.day}
                  </div>
                </div>

                {/* Event info */}
                <div>
                  <div
                    style={{
                      fontWeight: 700,
                      fontSize: "1.05rem",
                      color: "var(--s6-black)",
                      marginBottom: "0.25rem",
                    }}
                  >
                    Event {event.id}: {event.name}
                  </div>
                  <div
                    style={{
                      fontSize: "0.88rem",
                      color: "var(--text-muted)",
                      marginBottom: "0.5rem",
                    }}
                  >
                    {event.city}, {event.state}
                  </div>
                  <div
                    style={{ display: "flex", gap: "0.4rem", flexWrap: "wrap" }}
                  >
                    {event.divisions.map((div) => (
                      <span
                        key={div}
                        style={{
                          fontSize: "0.72rem",
                          fontWeight: 600,
                          letterSpacing: "0.05em",
                          padding: "0.2rem 0.55rem",
                          borderRadius: "3px",
                          background: "var(--s6-black)",
                          color: "var(--cream)",
                        }}
                      >
                        {div}
                      </span>
                    ))}
                  </div>
                </div>

                {/* CTA */}
                <div>
                  <Link
                    href="/register"
                    style={{
                      display: "inline-block",
                      padding: "0.6rem 1.1rem",
                      background: "var(--s6-orange)",
                      color: "var(--cream)",
                      fontWeight: 700,
                      fontSize: "0.82rem",
                      letterSpacing: "0.06em",
                      textTransform: "uppercase",
                      borderRadius: "4px",
                      textDecoration: "none",
                      whiteSpace: "nowrap",
                    }}
                  >
                    Register →
                  </Link>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Season package CTA */}
      <section
        style={{
          background: "var(--s6-black)",
          padding: "5rem 0",
          textAlign: "center",
        }}
      >
        <div className="container-xl" style={{ maxWidth: "640px" }}>
          <p className="editorial-eyebrow" style={{ color: "var(--s6-orange)" }}>
            Season Pass
          </p>
          <h2 style={{ color: "var(--cream)", marginBottom: "1rem" }}>
            All 10 events for <em>$899.</em>
          </h2>
          <p
            style={{
              color: "var(--cream)",
              opacity: 0.75,
              fontSize: "1.05rem",
              marginBottom: "2rem",
            }}
          >
            Lock in every event on the calendar. Three guaranteed games per
            tournament, four venues, one flat price.
          </p>
          <Link
            href="/register"
            className="btn-primary"
            style={{ fontSize: "1rem", padding: "0.9rem 2.2rem" }}
          >
            Get the Season Pass
          </Link>
        </div>
      </section>
    </main>
  );
}
