import Image from "next/image";
import Link from "next/link";

/* ─── Division Sections — One photo per section.
   Alternating photo / text splits. No clustering. Each
   photograph gets full-width prominence at hero quality. */

const divisions = [
  {
    label: "Boys Division",
    headline: "Boys, <em>12th–3rd grade.</em>",
    description:
      "Three levels of play — Elite, Competitive, and Developmental — at every Super 6 weekend. NFHS officials, custom court branding, college-pipeline access for top-tier teams.",
    image: "/media/curated/02-rebound-contest.jpg",
    objectPosition: "center 35%",
    imagePosition: "left" as const,
    href: "/register",
  },
  {
    label: "Girls Division",
    headline: "Girls, <em>12th–6th grade.</em>",
    description:
      "Three levels of competitive play with the same officiating standard, the same production, and the same college-pipeline introductions as the boys' bracket. Every weekend.",
    image: "/media/curated/23-female-athlete.jpg",
    objectPosition: "center center",
    imagePosition: "right" as const,
    href: "/register",
  },
  {
    label: "Elite Showcase",
    headline: "Elite. <em>Scout-visible.</em>",
    description:
      "Top-tier weekends where the best teams in the Southeast play in front of college coaches and recruiters. Limited spots, invite-priority, scout-credentialed.",
    image: "/media/curated/08-fast-break.jpg",
    objectPosition: "center 40%",
    imagePosition: "left" as const,
    href: "/register",
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
                alt={d.label}
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
                  Register your team
                </Link>
              </div>
            </div>
          </div>
        </section>
      ))}
    </>
  );
}
