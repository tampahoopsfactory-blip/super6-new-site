import type { Metadata } from "next";
import Link from "next/link";

export const metadata: Metadata = {
  title: "Officials | Super6 Basketball",
  description: "Super6 officiating information — coming soon.",
};

export default function OfficialsPage() {
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
            Officials
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
            Officiating registration, rules, and event assignments are coming.
            Questions in the meantime? Contact us directly.
          </p>
          <Link
            href="/contact"
            className="btn-primary"
            style={{ fontSize: "1rem", padding: "0.9rem 2rem" }}
          >
            Contact Us
          </Link>
        </div>
      </section>
    </main>
  );
}
