import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";

export const metadata: Metadata = {
  title: "Coaches | Super6 Basketball",
  description:
    "Super6 is built for coaches. Competitive divisions, organized events, and the exposure your players need to level up.",
};

const reasons = [
  {
    heading: "Competitive divisions that are actually competitive",
    body: "We bracket by age and skill — not just age. Your best players go against their best players. Every game means something.",
  },
  {
    heading: "Three guaranteed games per event",
    body: "No one-and-done. Every team plays a minimum of three games. Your players get real run, not just a single elimination exit.",
  },
  {
    heading: "Well-run facilities, real courts",
    body: "Gyms across Orlando, Tampa, Clearwater, West Palm Beach, and Atlanta. Professional setup, on-time schedules, no chaos.",
  },
  {
    heading: "Affordable entry — individual or full season",
    body: "$99 per event or $899 for the full 10-event season pass. Budget-friendly for club programs of any size.",
  },
  {
    heading: "Exposure that matters",
    body: "Super6 events attract scouts and evaluators. We put your players in front of the right people at the right stage of development.",
  },
  {
    heading: "A staff that respects your time",
    body: "Brackets posted early. Results tracked. No chasing down scoresheets. We handle the logistics so you can coach.",
  },
];

export default function CoachesPage() {
  return (
    <main>
      {/* Hero */}
      <section className="page-hero">
        <Image
          src="/media/curated/01-flagship-coach-pointer.jpg"
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
            #1 Tournament Organization in Florida · For Coaches
          </p>
          <h1>
            Built for the
            <br />
            <em>people who coach.</em>
          </h1>
          <p
            style={{
              color: "var(--cream)",
              fontSize: "1.15rem",
              maxWidth: "520px",
              marginTop: "1.25rem",
            }}
          >
            Super6 exists to give coaches the competition, structure, and
            exposure their programs deserve — without the headaches.
          </p>
        </div>
      </section>

      {/* Why Super6 for coaches */}
      <section style={{ padding: "5rem 0", background: "var(--cream)" }}>
        <div className="container-xl" style={{ maxWidth: "860px" }}>
          <p
            className="editorial-eyebrow"
            style={{ color: "var(--s6-orange)" }}
          >
            Why Super6
          </p>
          <h2 style={{ marginBottom: "3rem" }}>
            Six reasons coaches <em>come back</em> every season.
          </h2>
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(auto-fit, minmax(340px, 1fr))",
              gap: "2.5rem",
            }}
          >
            {reasons.map((item, i) => (
              <div key={i}>
                <div
                  style={{
                    fontSize: "0.7rem",
                    fontWeight: 700,
                    letterSpacing: "0.14em",
                    color: "var(--s6-orange)",
                    textTransform: "uppercase",
                    marginBottom: "0.5rem",
                  }}
                >
                  0{i + 1}
                </div>
                <h3
                  style={{
                    fontSize: "1.05rem",
                    fontWeight: 700,
                    color: "var(--s6-black)",
                    marginBottom: "0.5rem",
                  }}
                >
                  {item.heading}
                </h3>
                <p
                  style={{
                    fontSize: "0.95rem",
                    color: "var(--text-muted)",
                    lineHeight: 1.65,
                  }}
                >
                  {item.body}
                </p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Photo break */}
      <section
        style={{
          position: "relative",
          height: "420px",
          overflow: "hidden",
        }}
      >
        <Image
          src="/media/curated/25-coaches-clipboard.jpg"
          alt="Coach reviewing clipboard at Super6 tournament"
          fill
          quality={90}
          sizes="100vw"
          style={{ objectFit: "cover", objectPosition: "center 40%" }}
        />
        <div
          style={{
            position: "absolute",
            inset: 0,
            background: "rgba(15,15,15,0.45)",
          }}
        />
        <div
          className="container-xl"
          style={{
            position: "relative",
            zIndex: 1,
            height: "100%",
            display: "flex",
            alignItems: "center",
          }}
        >
          <blockquote
            style={{
              color: "var(--cream)",
              fontSize: "1.5rem",
              fontWeight: 300,
              fontStyle: "italic",
              maxWidth: "600px",
              lineHeight: 1.5,
              borderLeft: "3px solid var(--s6-orange)",
              paddingLeft: "1.5rem",
              margin: 0,
            }}
          >
            "The best-run youth basketball tournament I have been to in 20 years
            of coaching."
          </blockquote>
        </div>
      </section>

      {/* Season options */}
      <section
        style={{ padding: "5rem 0", background: "var(--cream)" }}
      >
        <div
          className="container-xl"
          style={{ maxWidth: "760px", textAlign: "center" }}
        >
          <p
            className="editorial-eyebrow"
            style={{ color: "var(--s6-orange)" }}
          >
            Register Your Team
          </p>
          <h2 style={{ marginBottom: "1rem" }}>
            Pick an event. <em>Or the whole season.</em>
          </h2>
          <p
            style={{
              color: "var(--text-muted)",
              fontSize: "1rem",
              marginBottom: "3rem",
            }}
          >
            Single-event entries at $99. Season pass for all 10 events at $899.
            Multi-team discounts available — contact us.
          </p>
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "1fr 1fr",
              gap: "1.5rem",
              maxWidth: "560px",
              margin: "0 auto",
            }}
          >
            <Link
              href="/register"
              style={{
                display: "block",
                padding: "1.25rem",
                border: "2px solid var(--s6-black)",
                borderRadius: "6px",
                textDecoration: "none",
                textAlign: "center",
              }}
            >
              <div
                style={{
                  fontSize: "1.5rem",
                  fontWeight: 700,
                  color: "var(--s6-black)",
                }}
              >
                $99
              </div>
              <div
                style={{
                  fontSize: "0.8rem",
                  textTransform: "uppercase",
                  letterSpacing: "0.1em",
                  color: "var(--text-muted)",
                  marginTop: "0.25rem",
                }}
              >
                Single Event
              </div>
            </Link>
            <Link
              href="/register"
              style={{
                display: "block",
                padding: "1.25rem",
                background: "var(--s6-black)",
                borderRadius: "6px",
                textDecoration: "none",
                textAlign: "center",
              }}
            >
              <div
                style={{
                  fontSize: "1.5rem",
                  fontWeight: 700,
                  color: "var(--s6-orange)",
                }}
              >
                $899
              </div>
              <div
                style={{
                  fontSize: "0.8rem",
                  textTransform: "uppercase",
                  letterSpacing: "0.1em",
                  color: "var(--cream)",
                  opacity: 0.7,
                  marginTop: "0.25rem",
                }}
              >
                Season Pass · All 10
              </div>
            </Link>
          </div>
        </div>
      </section>

      {/* Bottom CTA */}
      <section
        style={{
          background: "var(--s6-black)",
          padding: "5rem 0",
          textAlign: "center",
        }}
      >
        <div className="container-xl" style={{ maxWidth: "580px" }}>
          <h2 style={{ color: "var(--cream)", marginBottom: "1rem" }}>
            Ready to bring your team?
          </h2>
          <p
            style={{
              color: "var(--cream)",
              opacity: 0.7,
              marginBottom: "2rem",
              fontSize: "1.05rem",
            }}
          >
            Register online or reach us directly. We will get you in.
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
              className="btn-primary"
              style={{ fontSize: "1rem", padding: "0.9rem 2rem" }}
            >
              Register Your Team
            </Link>
            <Link
              href="/contact"
              style={{
                display: "inline-block",
                padding: "0.9rem 2rem",
                border: "2px solid rgba(255,255,255,0.3)",
                color: "var(--cream)",
                fontWeight: 600,
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
