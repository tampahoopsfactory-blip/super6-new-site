import type { Metadata } from "next";
import Link from "next/link";

export const metadata: Metadata = {
  title: "Shop | Super6 Basketball",
  description: "Super6 merchandise and gear — coming soon.",
  robots: { index: false, follow: false },
};

export default function ShopPage() {
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
            Shop
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
            Super6 gear and merchandise is on the way. Check back soon — or
            register your team while you&apos;re here.
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
