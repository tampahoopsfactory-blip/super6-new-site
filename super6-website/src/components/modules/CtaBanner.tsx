import Link from "next/link";

/* ─── Final CTA — Editorial dark close
   Warm-black background with subtle orange ambient glow.
   Serif headline with italic accent, restrained CTA pair. */

export default function CtaBanner() {
  return (
    <section className="cta-section" aria-label="Call to action">
      <div className="container-xl">
        <h2 className="cta-heading">
          Bring your team <em>this season.</em>
        </h2>
        <p className="cta-sub">
          A thousand teams across Florida and Georgia have played a Super 6
          weekend. Three-game guarantee. NFHS officials. A championship
          atmosphere from tip-off to trophy.
        </p>
        <div style={{ display: "flex", gap: 14, justifyContent: "center", flexWrap: "wrap" as const }}>
          <Link href="/register" className="btn btn-orange">
            Register your team
          </Link>
          <Link href="/contact" className="btn btn-outline-light">
            Talk to us
          </Link>
        </div>
      </div>
    </section>
  );
}
