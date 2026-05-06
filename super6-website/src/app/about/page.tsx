import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import { REGISTER_LINK_PROPS } from "@/lib/links";
import { siteSmsHref } from "@/data/site";
import AlumniSection from "@/components/modules/AlumniSection";

/* /about · Our Mission · editorial layout. Copy rule: .cursor/rules/super6-copy-no-dashes.mdc */

export const metadata: Metadata = {
  title: "Our Mission",
  description:
    "Super6 Series LLC was built on a simple idea. Every kid deserves a real shot, on the court, in the classroom, and in the life that comes after the buzzer.",
  alternates: { canonical: "/about" },
  openGraph: {
    title: "Our Mission | Super6 Series LLC",
    description:
      "Twelve years of premier youth basketball weekends paired with academic tutoring, college counseling, mentorship, and family centered programming.",
    url: "/about",
    type: "website",
  },
};

const pillars = [
  {
    number: "01",
    title: "Affordable access",
    body: "Cost should not decide who plays. We keep premier weekends in reach for serious families.",
  },
  {
    number: "02",
    title: "Academic tutoring",
    body: "Players grow when grades grow. We help athletes carry the books that carry the future.",
  },
  {
    number: "03",
    title: "College counseling",
    body: "The next step takes a guide. We help families read offers and protect smart choices.",
  },
  {
    number: "04",
    title: "Mentorship",
    body: "Coaches who keep showing up. Stories from people who walked the same road.",
  },
  {
    number: "05",
    title: "Family programming",
    body: "Parents and grandparents stay close. The team includes the people in the stands.",
  },
];

export default function AboutPage() {
  return (
    <>
      {/* HERO */}
      <section className="mission-hero">
        <div className="mission-hero-photo">
          <Image
            src="/media/uploads/about-mission-hero-team.jpg"
            alt="Super6 Series LLC team on the basketball court with branded banners"
            fill
            priority
            quality={94}
            sizes="100vw"
          />
        </div>
        <div className="container-xl mission-hero-content">
          <p className="mission-hero-eyebrow">Our Mission</p>
          <h1 className="mission-hero-headline">
            Every kid deserves a <em>real shot.</em>
          </h1>
          <p className="mission-hero-sub">
            On the court. In the classroom. In the life that comes after the buzzer.
          </p>
        </div>
      </section>

      {/* MISSION STATEMENT */}
      <section className="section mission-statement" aria-labelledby="mission-statement-anchor">
        <div className="container-xl mission-statement-inner">
          <p className="section-label">The Super6 idea</p>
          <h2 id="mission-statement-anchor" className="mission-lead">
            Super6 was built on a simple idea. Every kid deserves a real shot.
            On the court, in the classroom, and in the life that comes after
            the buzzer.
          </h2>
          <p className="mission-body">
            For twelve years we have run the Southeast&apos;s most competitive
            youth basketball weekends, packing premier facilities with serious
            teams, watchful scouts, and families who travel for the love of the
            game. The scoreboard is only half of what we do.
          </p>
          <blockquote className="mission-pullquote">
            <p>
              We close the gap between talent and opportunity by treating both
              as equal weights.
            </p>
          </blockquote>
          <p className="mission-body">
            Behind every Super6 event sits a longer mission. Affordable access.
            Academic tutoring. College counseling. Mentorship. Family centered
            programming designed to reach the kids the system tends to overlook.
          </p>
        </div>
      </section>

      {/* PILLARS */}
      <section className="section section-paper mission-pillars" aria-labelledby="mission-pillars-title">
        <div className="container-xl">
          <p className="section-label">The longer mission</p>
          <h2 id="mission-pillars-title" className="section-heading">
            Five things we work on <em>off the scoreboard.</em>
          </h2>
        </div>
        <ol className="mission-pillar-list" aria-label="Mission pillars">
          {pillars.map((p) => (
            <li key={p.number} className="mission-pillar">
              <span className="mission-pillar-num" aria-hidden="true">
                {p.number}
              </span>
              <h3 className="mission-pillar-title">{p.title}</h3>
              <p className="mission-pillar-body">{p.body}</p>
            </li>
          ))}
        </ol>
      </section>

      <AlumniSection />

      {/* CLOSER */}
      <section className="mission-closer" aria-labelledby="mission-closer-statement">
        <div className="container-xl mission-closer-inner">
          <p className="mission-closer-eyebrow">Our promise</p>
          <h2 id="mission-closer-statement" className="mission-closer-statement">
            Sports open the door.
            <br />
            <em>Super6 walks them through it.</em>
          </h2>
          <div className="mission-closer-actions">
            <Link
              {...REGISTER_LINK_PROPS}
              className="btn-hero btn-hero-primary"
            >
              Register your team
            </Link>
            <a href={siteSmsHref} className="btn-hero btn-hero-secondary">
              Text Super6
            </a>
          </div>
        </div>
      </section>
    </>
  );
}
