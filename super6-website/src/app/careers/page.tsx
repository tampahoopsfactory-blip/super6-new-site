import type { Metadata } from "next";
import { siteSmsHref } from "@/data/site";

export const metadata: Metadata = {
  title: "Careers | Super6 Series LLC Basketball",
  description: "Opportunities to work with Super6 Series LLC — coming soon.",
  robots: { index: false, follow: false },
};

export default function CareersPage() {
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
            Careers
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
            Open positions at Super6 Series LLC will be posted here. Interested in joining
            the team? Send us a message.
          </p>
          <a
            href={siteSmsHref}
            className="btn-primary"
            style={{ fontSize: "1rem", padding: "0.9rem 2rem" }}
          >
            Text Super6
          </a>
        </div>
      </section>
    </main>
  );
}
