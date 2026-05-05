import Image from "next/image";
import Link from "next/link";

/* ─── Division Sections — One photo per section.
   Alternating photo / text splits. No clustering. Each
   photograph gets full-width prominence at hero quality. */

type DivisionSection = {
  label: string;
  headline: string;
  description: string;
  image: string;
  objectPosition: string;
  imagePosition: "left" | "right";
  href: string;
  cta: string;
  /** Optional descriptive alt when `label` is not enough for screen readers. */
  imageAlt?: string;
};

const divisions: DivisionSection[] = [
  {
    label: "Boys & Girls Division",
    headline: "Boys & Girls, <em>12th–3rd grade.</em>",
    description:
      "Three levels of play — Elite, Competitive, and Developmental — at every Super 6 weekend. NFHS officials, custom court branding, college-pipeline access for top-tier teams.",
    image: "/media/uploads/boys-division-packed-house.jpg",
    objectPosition: "center center",
    imagePosition: "left" as const,
    href: "/register",
    cta: "Register your team",
  },
  {
    label: "Elite Showcase",
    headline: "Elite. <em>Scout-visible.</em>",
    description:
      "Top-tier weekends where the best teams in the Southeast play in front of college coaches and recruiters. Limited spots, invite-priority, scout-credentialed.",
    image: "/media/uploads/experience-thunder-boxout.jpg",
    objectPosition: "center center",
    imagePosition: "right" as const,
    href: "/register",
    cta: "Apply for Elite",
  },
  {
    label: "Walk-On Ready",
    headline: "One club. <em>One standard.</em>",
    description:
      "Every Super6 weekend, programs roll in from across Florida and the Southeast — jerseys pressed, shorts matching, athletes locked in before the ball goes up. We protect how the game looks on our floors so every club walks on looking like they belong.",
    image: "/media/uploads/super6-three-athletes-walk.png",
    objectPosition: "center 42%",
    imagePosition: "left",
    href: "/faq#uniform-requirements",
    cta: "Uniform expectations",
    imageAlt:
      "Three basketball athletes in matching team uniforms walking across the court at a Super6 event",
  },
];

export default function DivisionSections() {
  return (
    <>
      {/* Section intro */}
      <section className="section" aria-label="Find Your Competition">
        <div className="container-xl" style={{ textAlign: "center" }}>
          <p className="section-label" style={{ justifyContent: "center" }}>
            Find Your Competition
          </p>
          <h2 className="section-heading" style={{ margin: "0 auto 16px" }}>
            Every level. <em>Every division.</em>
          </h2>
          <p className="section-desc" style={{ margin: "0 auto" }}>
            Three brackets across every weekend. Pick the one that fits your team.
          </p>
        </div>
      </section>

      {/* One section per division */}
      {divisions.map((d, i) => (
        <section
          key={d.label}
          aria-label={d.label}
          className={i % 2 === 1 ? "split-warm" : ""}
        >
          <div className="split">
            <div
              className="split-image"
              style={{ order: d.imagePosition === "left" ? 0 : 1 }}
            >
              <Image
                src={d.image}
                alt={d.imageAlt ?? d.label}
                fill
                sizes="(max-width: 968px) 100vw, 50vw"
                quality={92}
                style={{ objectFit: "cover", objectPosition: d.objectPosition }}
              />
            </div>
            <div
              className="split-content"
              style={{ order: d.imagePosition === "left" ? 1 : 0 }}
            >
              <p className="section-label">{d.label}</p>
              <h2
                className="section-heading"
                dangerouslySetInnerHTML={{ __html: d.headline }}
              />
              <p className="section-desc" style={{ marginBottom: 36 }}>
                {d.description}
              </p>
              <div>
                <Link href={d.href} className="btn btn-ink">
                  {d.cta}
                </Link>
              </div>
            </div>
          </div>
        </section>
      ))}
    </>
  );
}
