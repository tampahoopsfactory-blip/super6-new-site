"use client";

import Image from "next/image";

/* ─── Experience Section — Editorial story moment
   Left: short feature list with serif sub-heads.
   Right: full-bleed photo with photo-grade. */

const features = [
  {
    title: "NFHS-Certified Officials",
    description:
      "Professional referees at every tournament. Fair, competitive play held to the highest standard in youth basketball.",
  },
  {
    title: "College Pipeline",
    description:
      "Direct partnerships with Ivy League programs, HBCUs, and top-tier colleges connect athletes to tutoring, test prep, and counseling.",
  },
  {
    title: "Three-Game Guarantee",
    description:
      "Every team plays a minimum of three games per tournament. No early elimination, maximum court time.",
  },
];

export default function ExperienceSection() {
  return (
    <section className="section section-warm" aria-label="The Super 6 Experience">
      <div className="container-xl">
        <div className="experience-grid">
          {/* Text side */}
          <div style={{ display: "flex", flexDirection: "column", justifyContent: "center" }}>
            <p className="section-label">Why Super 6</p>
            <h2 className="section-heading">
              Built for the moment <em>that matters.</em>
            </h2>
            <p className="section-desc" style={{ marginBottom: 48 }}>
              Twelve years refining what a youth tournament should feel like. The
              difference is in the details — the officiating, the access, the
              guarantee that every athlete gets real minutes.
            </p>
            <div style={{ display: "flex", flexDirection: "column", gap: 32 }}>
              {features.map((feature) => (
                <div key={feature.title}>
                  <h3
                    style={{
                      fontFamily: "var(--font-display)",
                      fontSize: 24,
                      fontWeight: 400,
                      letterSpacing: "-0.018em",
                      color: "var(--ink)",
                      marginBottom: 8,
                    }}
                  >
                    {feature.title}
                  </h3>
                  <p
                    style={{
                      fontSize: 16,
                      color: "var(--slate)",
                      lineHeight: 1.6,
                      maxWidth: "52ch",
                    }}
                  >
                    {feature.description}
                  </p>
                </div>
              ))}
            </div>
          </div>

          {/* Photo side — editorial 2-photo stack */}
          <div className="experience-photo-stack">
            <figure className="experience-photo-primary">
              <Image
                src="/media/uploads/refs-crew.jpg"
                alt="Super 6 officiating crew of four NFHS-certified referees with the Super 6 banner"
                fill
                sizes="(max-width: 968px) 100vw, 45vw"
                quality={92}
                style={{ objectFit: "cover" }}
              />
            </figure>
            <figure className="experience-photo-secondary">
              <Image
                src="/media/uploads/elite-referees.jpg"
                alt="Two NFHS-certified Super 6 referees holding the game ball"
                fill
                sizes="(max-width: 968px) 50vw, 22vw"
                quality={92}
                style={{ objectFit: "cover", objectPosition: "center 30%" }}
              />
            </figure>
          </div>
        </div>

      </div>
    </section>
  );
}
