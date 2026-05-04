import type { Metadata } from "next";
import Link from "next/link";

export const metadata: Metadata = {
  title: "Players | Super6 Basketball",
  description: "Super6 player resources — coming soon.",
  robots: { index: false, follow: false },
};

export default function PlayersPage() {
  return (
    <main>
      <section
        style={{
          background: "var(--s6-black)",
          minHeight: "60vh",
          display: "flex",
          alignItems: "center",
          padding: "6rem 0",
        }}
      >
        <div
          className="container-xl"
          style={{ maxWidth: "580px", textAlign: "center" }}
        >
          <p
            className="editorial-eyebrow"
            style={{ color: "var(--s6-orange)", marginBottom: "1rem" }}
          >
            Players
          </p>
          <h1 style={{ color: "var(--cream)", marginBottom: "1rem" }}>
            Coming soon.
          </h1>
          <p
            style={{
              color: "var(--cream)",
              opacity: 0.65,
              fontSize: "1.05rem",
              marginBottom: "2.5rem",
            }}
          >
            Player resources, stats, and profiles are on the way. In the
            meantime, find your next event and get on the court.
          </p>
          <Link
            href="/schedule"
            className="btn-primary"
            style={{ fontSize: "1rem", padding: "0.9rem 2rem" }}
          >
            View 2026 Schedule
          </Link>
        </div>
      </section>
    </main>
  );
}
