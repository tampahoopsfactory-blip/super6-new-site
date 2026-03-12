import Image from "next/image";
import Link from "next/link";

/* ─── Mission Split — 50/50 photo + text
   Anthropic spacing × Nike photography. Full-bleed image,
   generous padding on text side. Alternating layout. ─── */

const sections = [
  {
    label: "Our Mission",
    heading: "Bridging Education & Athletics",
    description:
      "At Super 6, our mission is to bridge the education gap for underserved youth and prepare them for lasting success beyond the playing field. Through partnerships with top-tier institutions — including Ivy League universities, HBCUs, and leading colleges — we connect student-athletes with tutoring, test preparation, and college counseling services.",
    cta: { label: "Learn More", href: "/about" },
    image: "/media/gallery/G2_14_Large_Group_Team.jpg",
    imagePosition: "left" as const,
  },
  {
    label: "The Experience",
    heading: "Championship Atmosphere",
    description:
      "From custom court branding and digital scorebooks to NFHS-certified referees and climate-controlled venues, every detail is designed to deliver a professional tournament experience. We bring a championship atmosphere to every weekend.",
    cta: { label: "View Locations", href: "/locations" },
    image: "/media/gallery/G1_11_Packed_Crowd.jpg",
    imagePosition: "right" as const,
  },
];

export default function MissionSplit() {
  return (
    <>
      {sections.map((section) => (
        <section
          key={section.heading}
          aria-label={section.heading}
          className={section.imagePosition === "right" ? "split-warm" : ""}
        >
          <div className="split">
            <div
              className="split-image"
              style={{ order: section.imagePosition === "left" ? 0 : 1 }}
            >
              <Image
                src={section.image}
                alt={section.heading}
                fill
                sizes="(max-width: 768px) 100vw, 50vw"
                className="object-cover"
              />
            </div>
            <div
              className="split-content"
              style={{ order: section.imagePosition === "left" ? 1 : 0 }}
            >
              <p className="section-label">{section.label}</p>
              <h2 className="section-heading">{section.heading}</h2>
              <p className="section-desc" style={{ marginBottom: "32px" }}>
                {section.description}
              </p>
              <div>
                <Link href={section.cta.href} className="btn btn-black">
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
