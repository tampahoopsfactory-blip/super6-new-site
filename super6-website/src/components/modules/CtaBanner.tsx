import Link from "next/link";

/* ─── Final CTA — Full-bleed dark section
   Under Armour: bold, monochromatic call to action.
   Centered text, pill CTA buttons. ─── */
export default function CtaBanner() {
  return (
    <section className="cta-section" aria-label="Call to action">
      <div className="container-xl">
        <h2 className="cta-heading">
          Ready to Compete?
        </h2>
        <p className="cta-sub">
          Join 1,000+ teams across Florida and Georgia. $99 entry.
          3-game guarantee. Championship atmosphere at every tournament.
        </p>
        <div style={{ display: "flex", gap: "12px", justifyContent: "center", flexWrap: "wrap" as const }}>
          <Link href="/register" className="btn btn-white">
            Register Your Team
          </Link>
          <Link href="/contact" className="btn btn-outline" style={{ color: "var(--white)", borderColor: "rgba(255,255,255,0.3)" }}>
            Contact Us
          </Link>
        </div>
      </div>
    </section>
  );
}
