import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";

export const metadata: Metadata = {
  title: "Programs | Super6 Basketball",
  description:
    "Super6 programs for every level — youth development, travel teams, school programs, and elite competition. Find where your players fit.",
};

const programs = [
  {
    id: "youth",
    eyebrow: "Youth Development",
    heading: "Building the foundation.",
    subheading: "3rd–5th grade",
    body: "Our youngest division is where basketball habits are formed. Super6 youth events are structured for maximum skill development — not just wins and losses. Coaches get real game reps, players get consistent competition, and programs build a foundation they carry through high school.",
    bullets: [
      "3rd–5th grade bracketing by skill, not just age",
      "Minimum 3 games per event",
      "Emphasis on development over elimination",
      "Qualified officials at every game",
    ],
    image: "/media/curated/03-crowd-sideline.jpg",
    imageAlt: "Young players competing at Super6 youth event",
  },
  {
    id: "travel",
    eyebrow: "Travel Teams",
    heading: "Real competition. Real exposure.",
    subheading: "6th–8th grade",
    body: "The middle school division is where programs separate. Super6 brackets travel teams against true competition — no mismatches, no lopsided games. Every team we put your players against is there to win. Scouts begin appearing at this level, and Super6 provides the platform.",
    bullets: [
      "Skill-based seeding within 6th–8th division",
      "Four venues across Florida and Atlanta",
      "Scouts and evaluators at select events",
      "Season pass saves programs $90 per event",
    ],
    image: "/media/curated/02-game-action-drive.jpg",
    imageAlt: "Middle school players in game action at Super6",
  },
  {
    id: "school",
    eyebrow: "School Programs",
    heading: "Compete beyond the school schedule.",
    subheading: "All divisions",
    body: "Super6 is built for school-based programs that need structured competition outside of their regular season. Our weekend format fits school schedules, our pricing fits school budgets, and our venues are within driving distance of every major Florida market. Bring your JV, varsity, or middle school squad.",
    bullets: [
      "Weekend events — no school-day conflicts",
      "$99 per event, $899 for all 10",
      "Multi-team discounts available",
      "Brackets organized by division and skill",
    ],
    image: "/media/curated/04-team-huddle.jpg",
    imageAlt: "Team huddle during Super6 tournament",
  },
  {
    id: "elite",
    eyebrow: "Elite Competition",
    heading: "The highest level we offer.",
    subheading: "9th–12th grade",
    body: "The high school division at Super6 is where college scouts come to evaluate. We put the best programs in Florida in the same bracket and let them play. If your players are ready for the next level, this is where they prove it. Exposure is built through performance, and Super6 is the stage.",
    bullets: [
      "9th–12th grade — full high school division",
      "Scouts and evaluators at every event",
      "Competitive seeding — no weak pools",
      "Championship awards for division finalists",
    ],
    image: "/media/curated/06-dunk-attempt.jpg",
    imageAlt: "High school player at Super6 elite event",
  },
];

export default function ProgramsPage() {
  return (
    <main>
      {/* Hero */}
      <section className="page-hero">
        <Image
          src="/media/curated/09-defensive-stop.jpg"
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
            #1 Tournament Organization in Florida · Programs
          </p>
          <h1>
            Every level.
            <br />
            <em>Every program.</em>
          </h1>
          <p
            style={{
              color: "var(--cream)",
              fontSize: "1.15rem",
              maxWidth: "520px",
              marginTop: "1.25rem",
            }}
          >
            From youth development to elite high school competition — Super6
            has the right division for every team you bring.
          </p>
        </div>
      </section>

      {/* Programs — alternating layout */}
      {programs.map((program, i) => (
        <section
          key={program.id}
          id={program.id}
          style={{
            padding: "5rem 0",
            background: i % 2 === 0 ? "var(--cream)" : "var(--s6-black)",
          }}
        >
          <div
            className="container-xl"
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(auto-fit, minmax(320px, 1fr))",
              gap: "3.5rem",
              alignItems: "center",
            }}
          >
            {/* Text — alternates left/right */}
            <div style={{ order: i % 2 === 0 ? 0 : 1 }}>
              <p
                className="editorial-eyebrow"
                style={{
                  color: "var(--s6-orange)",
                  marginBottom: "0.25rem",
                }}
              >
                {program.eyebrow}
              </p>
              <p
                style={{
                  fontSize: "0.82rem",
                  fontWeight: 600,
                  letterSpacing: "0.08em",
                  textTransform: "uppercase",
                  color: i % 2 === 0 ? "var(--text-muted)" : "rgba(255,255,255,0.5)",
                  marginBottom: "0.75rem",
                }}
              >
                {program.subheading}
              </p>
              <h2
                style={{
                  color: i % 2 === 0 ? "var(--s6-black)" : "var(--cream)",
                  marginBottom: "1rem",
                  fontSize: "1.85rem",
                }}
              >
                {program.heading}
              </h2>
              <p
                style={{
                  fontSize: "0.97rem",
                  lineHeight: 1.75,
                  color: i % 2 === 0 ? "var(--text-muted)" : "rgba(255,255,255,0.7)",
                  marginBottom: "1.5rem",
                }}
              >
                {program.body}
              </p>
              <ul style={{ margin: 0, padding: 0, listStyle: "none" }}>
                {program.bullets.map((b, bi) => (
                  <li
                    key={bi}
                    style={{
                      display: "flex",
                      gap: "0.6rem",
                      alignItems: "flex-start",
                      padding: "0.4rem 0",
                      fontSize: "0.9rem",
                      color: i % 2 === 0 ? "var(--text-muted)" : "rgba(255,255,255,0.65)",
                    }}
                  >
                    <span
                      style={{
                        color: "var(--s6-orange)",
                        fontWeight: 700,
                        flexShrink: 0,
                      }}
                    >
                      →
                    </span>
                    {b}
                  </li>
                ))}
              </ul>
            </div>

            {/* Image */}
            <div
              style={{
                position: "relative",
                height: "380px",
                borderRadius: "6px",
                overflow: "hidden",
                order: i % 2 === 0 ? 1 : 0,
              }}
            >
              <Image
                src={program.image}
                alt={program.imageAlt}
                fill
                sizes="(max-width: 768px) 100vw, 50vw"
                style={{ objectFit: "cover" }}
              />
            </div>
          </div>
        </section>
      ))}

      {/* Bottom CTA */}
      <section
        style={{
          background: "var(--s6-orange)",
          padding: "5rem 0",
          textAlign: "center",
        }}
      >
        <div className="container-xl" style={{ maxWidth: "560px" }}>
          <h2
            style={{
              color: "var(--s6-black)",
              marginBottom: "1rem",
            }}
          >
            Find your division. Register today.
          </h2>
          <p
            style={{
              color: "var(--s6-black)",
              opacity: 0.75,
              fontSize: "1.05rem",
              marginBottom: "2rem",
            }}
          >
            $99 per event. $899 for all 10. Multi-team discounts available.
          </p>
          <div
            style={{
              display: "flex",
              gap: "1rem",
              justifyContent: "center",
              flexWrap: "wrap",
            }}
          >
            <Link
              href="/register"
              style={{
                display: "inline-block",
                padding: "0.9rem 2rem",
                background: "var(--s6-black)",
                color: "var(--cream)",
                fontWeight: 700,
                fontSize: "1rem",
                borderRadius: "4px",
                textDecoration: "none",
              }}
            >
              Register Your Team
            </Link>
            <Link
              href="/contact"
              style={{
                display: "inline-block",
                padding: "0.9rem 2rem",
                border: "2px solid var(--s6-black)",
                color: "var(--s6-black)",
                fontWeight: 700,
                fontSize: "1rem",
                borderRadius: "4px",
                textDecoration: "none",
              }}
            >
              Contact Us
            </Link>
          </div>
        </div>
      </section>
    </main>
  );
}
