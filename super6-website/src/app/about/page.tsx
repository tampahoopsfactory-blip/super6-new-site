import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";

export const metadata: Metadata = {
  title: "About",
  description:
    "Super 6 is the Southeast's premier youth basketball tournament series. Twelve years of championship-level competition with college pipeline access and professional production.",
};

const values = [
  {
    title: "Competition",
    description:
      "Championship-caliber tournaments that challenge young athletes to compete at the highest level.",
  },
  {
    title: "Community",
    description:
      "Building connections between teams, coaches, and families across the Southeast basketball community.",
  },
  {
    title: "Experience",
    description:
      "Climate-controlled venues, digital scorebooks, custom court branding, NFHS-certified officials. Production at every weekend.",
  },
  {
    title: "Integrity",
    description:
      "Strict eligibility rules. NFHS-certified officials. Zero tolerance for unsportsmanlike conduct.",
  },
  {
    title: "Development",
    description:
      "Skills, discipline, competitive drive — the things that show up later. The college pipeline is the receipt.",
  },
  {
    title: "Passion",
    description:
      "Founded by people who love basketball and believe in the power of youth sports to shape lives.",
  },
];

const photoGrid = [
  "/media/curated/01-flagship-coach-pointer.jpg",
  "/media/curated/05-crowd-eruption.jpg",
  "/media/curated/09-trophy-raise.jpg",
  "/media/curated/03-drive-isolation.jpg",
  "/media/curated/15-kids-whiteboard.jpg",
  "/media/curated/12-coach-intensity.jpg",
  "/media/curated/17-young-spectators.jpg",
  "/media/curated/16-packed-sideline.jpg",
];

export default function AboutPage() {
  return (
    <>
      {/* Hero */}
      <section className="page-hero">
        <Image
          src="/media/curated/06-super6-banner-bokeh.jpg"
          alt=""
          fill
          priority
          sizes="100vw"
          aria-hidden="true"
        />
        <div className="container-xl">
          <p className="editorial-eyebrow" style={{ color: "var(--cream)", opacity: 0.85 }}>
            Our Story
          </p>
          <h1>The Southeast&rsquo;s premier <em>youth basketball</em> tournament series.</h1>
          <p>
            Twelve years building competitive weekends where young athletes
            grow, families gather, and the next college recruit gets seen.
          </p>
        </div>
      </section>

      {/* Mission */}
      <section className="section">
        <div className="container-xl">
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "1fr",
              gap: 64,
              alignItems: "center",
            }}
          >
            <div style={{ maxWidth: 720 }}>
              <p className="section-label">Our Mission</p>
              <h2 className="section-heading">
                Elevating youth basketball <em>from the floor up.</em>
              </h2>
              <div style={{ display: "flex", flexDirection: "column", gap: 18, maxWidth: "60ch" }}>
                <p className="section-desc">
                  Super 6 was founded on a simple belief: young athletes deserve
                  a championship-level experience. From professional referees
                  and digital scorebooks to climate-controlled venues, every
                  detail is designed to bring the best out of competitors.
                </p>
                <p className="section-desc">
                  Operating across five markets in Florida and Georgia, Super 6
                  brings together the Southeast&rsquo;s most talented youth
                  teams for tournament weekends that follow NFHS standards and
                  put sportsmanship first.
                </p>
                <p className="section-desc">
                  Whether it&rsquo;s a third grader&rsquo;s first tournament or
                  a senior&rsquo;s last weekend, Super 6 is the stage where
                  young athletes find out what they&rsquo;re made of.
                </p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Big quote / split */}
      <section className="split-warm">
        <div className="split">
          <div className="split-image">
            <Image
              src="/media/curated/01-flagship-coach-pointer.jpg"
              alt="Coach mentoring a player on the sideline"
              fill
              sizes="(max-width: 968px) 100vw, 50vw"
              className="object-cover"
            />
          </div>
          <div className="split-content">
            <p className="section-label">Why It Matters</p>
            <h2 className="section-heading">
              The work doesn&rsquo;t end <em>when the buzzer does.</em>
            </h2>
            <p className="section-desc" style={{ marginBottom: 24 }}>
              We partner with Ivy League programs, HBCUs, and top-tier colleges
              to give athletes a path beyond the trophy &mdash; tutoring, test
              prep, college counseling, the kind of access that decides
              futures.
            </p>
            <div>
              <Link href="/contact" className="btn btn-ink">Talk to us</Link>
            </div>
          </div>
        </div>
      </section>

      {/* Values Grid */}
      <section className="section section-paper">
        <div className="container-xl">
          <div style={{ marginBottom: 56 }}>
            <p className="section-label">What Drives Us</p>
            <h2 className="section-heading">Six values, <em>one weekend at a time.</em></h2>
          </div>
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(auto-fit, minmax(280px, 1fr))",
              gap: 32,
            }}
          >
            {values.map((v) => (
              <div
                key={v.title}
                style={{
                  paddingLeft: 24,
                  borderLeft: "1px solid var(--hairline)",
                }}
              >
                <h3
                  style={{
                    fontFamily: "var(--font-display)",
                    fontSize: 24,
                    fontWeight: 400,
                    letterSpacing: "-0.018em",
                    color: "var(--ink)",
                    marginBottom: 10,
                  }}
                >
                  {v.title}
                </h3>
                <p style={{ fontSize: 15, color: "var(--slate)", lineHeight: 1.6 }}>
                  {v.description}
                </p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Photo grid */}
      <section className="section section-warm">
        <div className="container-xl" style={{ marginBottom: 48 }}>
          <p className="section-label">Game Day</p>
          <h2 className="section-heading">Twelve years <em>in frames.</em></h2>
        </div>
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(2, 1fr)",
            gap: 4,
          }}
          className="about-photo-grid"
        >
          {photoGrid.map((src) => (
            <div
              key={src}
              style={{
                position: "relative",
                aspectRatio: "1 / 1",
                overflow: "hidden",
                background: "var(--cream-warm)",
              }}
            >
              <Image
                src={src}
                alt="Super 6 game day moment"
                fill
                sizes="(max-width: 768px) 50vw, 25vw"
                style={{
                  objectFit: "cover",
                  filter: "var(--photo-grade)",
                  transition: "transform 1.4s var(--ease)",
                }}
              />
            </div>
          ))}
        </div>
      </section>

      {/* CTA */}
      <section className="cta-section" aria-label="Join us">
        <div className="container-xl">
          <h2 className="cta-heading">
            Be part of <em>the story.</em>
          </h2>
          <p className="cta-sub">
            Coaches, parents, athletes, partners. There&rsquo;s a place for you
            at Super 6.
          </p>
          <div style={{ display: "flex", gap: 14, justifyContent: "center", flexWrap: "wrap" }}>
            <Link href="/register" className="btn btn-orange">Register your team</Link>
            <Link href="/contact" className="btn btn-outline-light">Get in touch</Link>
          </div>
        </div>
      </section>
    </>
  );
}
