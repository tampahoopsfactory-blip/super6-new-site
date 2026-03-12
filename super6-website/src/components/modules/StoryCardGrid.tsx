"use client";

import Image from "next/image";

/* ─── Experience Section — Video + production story
   Anthropic layout: generous grid, clean typography.
   Left: video embed. Right: feature highlights. ─── */

const features = [
  {
    title: "NFHS-Certified Officials",
    description:
      "Professional referees at every tournament ensure fair, competitive play at the highest standard.",
  },
  {
    title: "College Pipeline",
    description:
      "Partnerships with Ivy League, HBCUs, and top colleges connect athletes to tutoring, test prep, and counseling.",
  },
  {
    title: "3-Game Guarantee",
    description:
      "Every team plays a minimum of three games per tournament — no early elimination, maximum court time.",
  },
];

export default function ExperienceSection() {
  return (
    <section className="section" style={{ background: "var(--surface)" }} aria-label="The Super 6 Experience">
      <div className="container-xl">
        <div style={{ textAlign: "center", marginBottom: "64px" }}>
          <p className="section-label">Why Super 6</p>
          <h2 className="section-heading" style={{ margin: "0 auto" }}>
            The Championship Experience
          </h2>
        </div>

        <div className="experience-grid">
          {/* Video side */}
          <div className="experience-video">
            <video
              autoPlay
              loop
              muted
              playsInline
              poster="/media/videos/hero-clip-4-poster.jpg"
            >
              <source src="/media/videos/hero-clip-4.mp4" type="video/mp4" />
            </video>
          </div>

          {/* Features side */}
          <div style={{ display: "flex", flexDirection: "column", justifyContent: "center", gap: "32px" }}>
            {features.map((feature) => (
              <div key={feature.title}>
                <h3 style={{
                  fontSize: "18px",
                  fontWeight: 500,
                  color: "var(--white)",
                  marginBottom: "8px",
                  letterSpacing: "-0.01em",
                }}>
                  {feature.title}
                </h3>
                <p style={{
                  fontSize: "15px",
                  color: "var(--gray-700)",
                  lineHeight: 1.6,
                }}>
                  {feature.description}
                </p>
              </div>
            ))}
          </div>
        </div>

        {/* Photo strip below */}
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(4, 1fr)",
          gap: "4px",
          marginTop: "64px",
        }}>
          {[
            "/media/gallery/G1_04_Courtside_Coaching.jpg",
            "/media/gallery/G2_05_Three_Staff_Celebration.jpg",
            "/media/gallery/G1_06_Youth_Action_SHOWOUT.jpg",
            "/media/gallery/G1_20_Halftime_Celebration.jpg",
          ].map((src) => (
            <div key={src} className="gallery-item" style={{ position: "relative", aspectRatio: "16/10", overflow: "hidden" }}>
              <Image
                src={src}
                alt="Super 6 tournament moment"
                fill
                sizes="25vw"
                className="object-cover"
              />
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}
