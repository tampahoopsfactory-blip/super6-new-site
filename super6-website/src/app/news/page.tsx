import type { Metadata } from "next";
import Link from "next/link";

export const metadata: Metadata = {
  title: "News | Super6 Basketball",
  description:
    "Super6 news, updates, and announcements — season schedules, new venues, event results, and program expansions.",
};

const articles = [
  {
    date: "April 28, 2026",
    category: "Season",
    title: "2026 Season Schedule Finalized — 10 Events Across 4 Markets",
    excerpt:
      "The full 2026 Super6 calendar is set. Ten events running May through December across Orlando, Tampa, Clearwater, West Palm Beach, and Atlanta. Season passes are now on sale.",
    slug: "2026-season-schedule",
  },
  {
    date: "April 15, 2026",
    category: "Venue",
    title: "Atlanta Invitational Returns for Second Straight Year",
    excerpt:
      "The Atlanta stop — one of the fastest-growing events on the Super6 calendar — returns in October 2026. Registration is already tracking ahead of last year.",
    slug: "atlanta-invitational-2026",
  },
  {
    date: "April 3, 2026",
    category: "Registration",
    title: "Season Pass Now Available — Lock In All 10 Events for $899",
    excerpt:
      "The 2026 season pass is live. One flat price covers every event on the calendar. Programs with multiple teams can contact us for volume pricing before registering.",
    slug: "season-pass-2026",
  },
  {
    date: "March 20, 2026",
    category: "Results",
    title: "2025 Season Finals Results — Champions Crowned in Orlando",
    excerpt:
      "The 2025 season came to a close in Orlando with three divisions, three champions, and a gym full of basketball. Full results and bracket breakdowns inside.",
    slug: "2025-season-finals-results",
  },
  {
    date: "March 5, 2026",
    category: "Program",
    title: "Super6 Expands to West Palm Beach for 2026",
    excerpt:
      "The Palm Beach Showcase in July 2026 marks Super6's first event in the West Palm Beach market. Registration is open now for all three divisions.",
    slug: "west-palm-beach-expansion",
  },
  {
    date: "February 18, 2026",
    category: "Season",
    title: "2026 Coaching Roster Tops 1,500 Programs",
    excerpt:
      "Super6 enters the 2026 season with more than 1,500 coaches in its network across Florida and Georgia. Registration for May's Spring Kickoff opens March 1.",
    slug: "coaching-network-2026",
  },
];

const categoryColor: Record<string, string> = {
  Season: "var(--s6-orange)",
  Venue: "#3B82F6",
  Registration: "#10B981",
  Results: "#8B5CF6",
  Program: "#F59E0B",
};

export default function NewsPage() {
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
            News & Updates
          </p>
          <h1 style={{ color: "var(--cream)" }}>
            What&apos;s happening
            <br />
            <em>at Super6.</em>
          </h1>
        </div>
      </section>

      {/* Article list */}
      <section style={{ padding: "5rem 0", background: "var(--cream)" }}>
        <div className="container-xl" style={{ maxWidth: "780px" }}>
          <div style={{ display: "flex", flexDirection: "column" }}>
            {articles.map((article, i) => (
              <div
                key={i}
                style={{
                  padding: "2rem 0",
                  borderBottom:
                    i < articles.length - 1
                      ? "1px solid var(--hairline-soft)"
                      : "none",
                }}
              >
                <div
                  style={{
                    display: "flex",
                    gap: "1rem",
                    alignItems: "center",
                    marginBottom: "0.75rem",
                    flexWrap: "wrap",
                  }}
                >
                  <span
                    style={{
                      fontSize: "0.7rem",
                      fontWeight: 700,
                      letterSpacing: "0.1em",
                      textTransform: "uppercase",
                      padding: "0.2rem 0.6rem",
                      borderRadius: "3px",
                      background: categoryColor[article.category] || "var(--s6-black)",
                      color: "var(--cream)",
                    }}
                  >
                    {article.category}
                  </span>
                  <span
                    style={{
                      fontSize: "0.82rem",
                      color: "var(--text-muted)",
                    }}
                  >
                    {article.date}
                  </span>
                </div>
                <h2
                  style={{
                    fontSize: "1.15rem",
                    fontWeight: 700,
                    color: "var(--s6-black)",
                    marginBottom: "0.6rem",
                    lineHeight: 1.35,
                  }}
                >
                  {article.title}
                </h2>
                <p
                  style={{
                    fontSize: "0.93rem",
                    color: "var(--text-muted)",
                    lineHeight: 1.65,
                    marginBottom: "0.75rem",
                  }}
                >
                  {article.excerpt}
                </p>
                <Link
                  href={`/news/${article.slug}`}
                  style={{
                    fontSize: "0.82rem",
                    fontWeight: 700,
                    color: "var(--s6-orange)",
                    textDecoration: "none",
                    letterSpacing: "0.04em",
                    textTransform: "uppercase",
                  }}
                >
                  Read more →
                </Link>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Stay updated */}
      <section
        style={{
          background: "var(--s6-black)",
          padding: "5rem 0",
          textAlign: "center",
        }}
      >
        <div className="container-xl" style={{ maxWidth: "520px" }}>
          <h2 style={{ color: "var(--cream)", marginBottom: "1rem" }}>
            Stay in the loop.
          </h2>
          <p
            style={{
              color: "var(--cream)",
              opacity: 0.7,
              fontSize: "1.05rem",
              marginBottom: "2rem",
            }}
          >
            Get event announcements, bracket drops, and Super6 updates directly
            to your inbox.
          </p>
          <Link
            href="/contact"
            className="btn-primary"
            style={{ fontSize: "1rem", padding: "0.9rem 2rem" }}
          >
            Get Updates
          </Link>
        </div>
      </section>
    </main>
  );
}
