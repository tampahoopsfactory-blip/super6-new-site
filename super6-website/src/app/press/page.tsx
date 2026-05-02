import type { Metadata } from "next";
import Link from "next/link";

export const metadata: Metadata = {
  title: "Press | Super6 Basketball",
  description: "Super6 press resources and media kit — coming soon.",
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
            requests, contact us directly.
          </p>
          <Link
            href="/contact"
            className="btn-primary"
            style={{ fontSize: "1rem", padding: "0.9rem 2rem" }}
          >
            Media Inquiries
          </Link>
        </div>
      </section>
    </main>
  );
}
