import type { Metadata } from "next";
import { siteSmsHref } from "@/data/site";

export const metadata: Metadata = {
  title: "Press | Super6 Series LLC Basketball",
  description: "Super6 Series LLC press resources and media kit — coming soon.",
  robots: { index: false, follow: false },
};

export default function PressPage() {
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
            Press
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
            Press kit, media assets, and coverage inquiries. For immediate media
            requests, text Super6 directly.
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
