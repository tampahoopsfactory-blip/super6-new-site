import Image from "next/image";
import Link from "next/link";

/* ─── Division Cards — Editorial 3-up category cards
   Full-bleed photos with overlay; serif title + small caps subtitle.
   Pure photographic moments, no chrome. */

const divisions = [
  {
    label: "Boys Division",
    subtitle: "12th–3rd Grade · Elite, Competitive, Developmental",
    image: "/media/curated/04-defensive-stance.jpg",
    href: "/register",
  },
  {
    label: "Girls Division",
    subtitle: "12th–6th Grade · Three Levels of Play",
    image: "/media/curated/13-team-king-crown.jpg",
    href: "/register",
  },
  {
    label: "Elite Showcase",
    subtitle: "Top-Tier Competition · Scout-Visible Weekends",
    image: "/media/curated/08-fast-break.jpg",
    href: "/register",
  },
];

export default function DivisionCards() {
  return (
    <section className="section" aria-label="Divisions">
      <div className="container-xl" style={{ marginBottom: 56 }}>
        <p className="section-label">Find Your Competition</p>
        <h2 className="section-heading" style={{ maxWidth: "20ch" }}>
          Every level. <em>Every division.</em>
        </h2>
      </div>

      <div className="container-xl">
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(auto-fit, minmax(280px, 1fr))",
            gap: 4,
          }}
        >
          {divisions.map((d) => (
            <Link key={d.label} href={d.href} className="card">
              <Image
                src={d.image}
                alt={d.label}
                fill
                sizes="(max-width: 768px) 100vw, 33vw"
                className="object-cover"
              />
              <div className="card-overlay" />
              <div className="card-content">
                <p className="card-label">{d.subtitle}</p>
                <h3 className="card-title">{d.label}</h3>
              </div>
            </Link>
          ))}
        </div>
      </div>
    </section>
  );
}
