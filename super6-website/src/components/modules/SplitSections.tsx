import Image from "next/image";
import Link from "next/link";

/* ─── Mission Split — 50/50 photo + editorial text
   Alternating layout. Photo grade applied via CSS. */

const sections = [
  {
    label: "Our Mission",
    heading: "Bridging education <em>and athletics.</em>",
    description:
      "Super 6 was founded in 2014 to bridge the education gap for underserved and developmental-skill youth — through programs that stress the importance of high-academic education to prepare kids for life well after sports. We reach our students through sport. Through partnerships with Ivy League, HBCU, and traditional colleges, we provide the most relevant tutoring, SAT/ACT prep, and academic college counseling.",
    cta: { label: "Learn more", href: "/about" },
    image: "/media/uploads/mission-team.jpg",
    imagePosition: "left" as const,
  },
  {
    label: "The Experience",
    heading: "A championship <em>atmosphere.</em>",
    description:
      "Custom court branding. Digital scorebooks. NFHS-certified referees. Climate-controlled venues. Every detail is engineered so that families and athletes feel the weight of the weekend — the moment the doors open.",
    cta: { label: "View locations", href: "/locations" },
    image: "/media/uploads/coach-huddle.jpg",
    imagePosition: "right" as const,
  },
];

export default function MissionSplit() {
  return (
    <>
      {sections.map((section) => (
        <section
          key={section.label}
          aria-label={section.label}
          className={section.imagePosition === "right" ? "split-warm" : ""}
        >
          <div className="split">
            <div
              className="split-image"
              style={{ order: section.imagePosition === "left" ? 0 : 1 }}
            >
              <Image
                src={section.image}
                alt={section.label}
                fill
                sizes="(max-width: 968px) 100vw, 50vw"
                className="object-cover"
                quality={92}
              />
            </div>
            <div
              className="split-content"
              style={{ order: section.imagePosition === "left" ? 1 : 0 }}
            >
              <p className="section-label">{section.label}</p>
              <h2
                className="section-heading"
                dangerouslySetInnerHTML={{ __html: section.heading }}
              />
              <p className="section-desc" style={{ marginBottom: 36 }}>
                {section.description}
              </p>
              <div>
                <Link href={section.cta.href} className="btn btn-ink">
                  {section.cta.label}
                </Link>
              </div>
            </div>
          </div>
        </section>
      ))}
    </>
  );
}
