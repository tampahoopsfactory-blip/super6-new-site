import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import { Trophy, Users, Video, Shield, Target, Heart } from "lucide-react";

export const metadata: Metadata = {
  title: "About",
  description:
    "Super6 is the premier youth basketball tournament series in the Southeast, delivering championship-level competition with professional production values.",
};

const values = [
  {
    icon: Trophy,
    title: "Competition",
    description:
      "Championship-caliber tournaments that challenge young athletes to compete at the highest level.",
  },
  {
    icon: Users,
    title: "Community",
    description:
      "Building connections between teams, coaches, and families across the Southeast basketball community.",
  },
  {
    icon: Video,
    title: "Experience",
    description:
      "Climate-controlled venues, digital scorebooks, custom court branding, and a championship atmosphere at every event.",
  },
  {
    icon: Shield,
    title: "Integrity",
    description:
      "NFHS-certified referees, strict eligibility rules, and a zero-tolerance policy for unsportsmanlike conduct.",
  },
  {
    icon: Target,
    title: "Development",
    description:
      "Creating an environment where young athletes develop skills, discipline, and competitive drive.",
  },
  {
    icon: Heart,
    title: "Passion",
    description:
      "Founded by people who love basketball and believe in the power of youth sports to shape lives.",
  },
];

export default function AboutPage() {
  return (
    <>
      {/* Hero */}
      <section className="relative bg-tm-black pt-32 pb-20">
        <div className="absolute inset-0">
          <Image
            src="/media/lifestyle/trophy-celebration.jpg"
            alt="Super6 championship celebration"
            fill
            className="object-cover opacity-20"
            priority
          />
        </div>
        <div className="absolute inset-0 bg-gradient-to-b from-black/60 to-tm-black" />
        <div className="tm-container relative">
          <p
            className="mb-4 text-xs font-medium tracking-widest uppercase text-white/60"

          >
            Our Story
          </p>
          <h1
            className="mb-4 text-4xl font-semibold tracking-tight text-white md:text-6xl"

          >
            About Super6
          </h1>
          <p className="max-w-xl text-base text-white/60 leading-relaxed">
            The Southeast&apos;s premier youth basketball tournament series.
            Where young athletes compete, grow, and build memories that last a
            lifetime.
          </p>
        </div>
      </section>

      {/* Mission */}
      <section className="tm-section bg-tm-bg">
        <div className="tm-container">
          <div className="grid gap-12 lg:grid-cols-2 lg:items-center">
            <div>
              <p
                className="mb-4 text-xs font-medium tracking-widest uppercase text-tm-muted"
    
              >
                Our Mission
              </p>
              <h2
                className="mb-6 text-2xl md:text-[32px] font-semibold tracking-tight text-tm-body leading-tight"
    
              >
                Elevating Youth Basketball
              </h2>
              <div className="space-y-4 text-sm md:text-base leading-relaxed text-tm-muted">
                <p>
                  Super6 was founded with a simple belief: young athletes deserve
                  a championship-level experience. From professional referees and
                  digital scorebooks to climate-controlled venues and
                  scorebooks, every detail is designed to bring out the best in
                  young competitors.
                </p>
                <p>
                  Operating across five locations in Florida and Georgia, Super6
                  brings together the Southeast&apos;s most talented youth teams
                  for competitive tournament weekends. Our format follows NFHS
                  rules, with strict eligibility standards and a code of conduct
                  that puts sportsmanship first.
                </p>
                <p>
                  Whether it&apos;s a third-grader&apos;s first tournament or a
                  high school senior&apos;s final season, Super6 creates the
                  stage where young athletes discover what they&apos;re made of.
                </p>
              </div>
            </div>
            <div className="tm-img-zoom overflow-hidden">
              <div className="relative aspect-[4/3]">
                <Image
                  src="/media/gallery/G1_06_Youth_Action_SHOWOUT.jpg"
                  alt="Youth basketball game action"
                  fill
                  sizes="(max-width: 1024px) 100vw, 50vw"
                  className="object-cover"
                />
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Values Grid */}
      <section className="tm-section bg-tm-bg-warm">
        <div className="tm-container">
          <div className="mb-12 text-center">
            <h2
              className="mb-4 text-2xl md:text-3xl font-semibold tracking-tight text-tm-body"
  
            >
              What Drives Us
            </h2>
          </div>
          <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
            {values.map((val) => (
              <div
                key={val.title}
                className="bg-tm-bg-alt p-8 transition-shadow hover:shadow-lg border border-tm-border"
              >
                <div className="mb-4 flex h-12 w-12 items-center justify-center bg-tm-bg">
                  <val.icon size={24} className="text-s6-orange" />
                </div>
                <h3
                  className="mb-2 text-sm font-semibold tracking-tight text-tm-body"
      
                >
                  {val.title}
                </h3>
                <p className="text-sm leading-relaxed text-tm-muted">
                  {val.description}
                </p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Photo Grid */}
      <section className="tm-section bg-tm-bg">
        <div className="tm-container">
          <div className="mb-12 text-center">
            <h2
              className="mb-4 text-2xl md:text-3xl font-semibold tracking-tight text-tm-body"
  
            >
              Game Day Moments
            </h2>
          </div>
          <div className="grid gap-2 sm:grid-cols-2 lg:grid-cols-4">
            {[
              { src: "/media/lifestyle/youth-action.jpg", alt: "Youth game action" },
              { src: "/media/lifestyle/defensive-play.jpg", alt: "Defensive intensity" },
              { src: "/media/lifestyle/three-point-shot.jpg", alt: "Three point shot" },
              { src: "/media/lifestyle/spectators-family.jpg", alt: "Families watching" },
              { src: "/media/lifestyle/award-ceremony.jpg", alt: "Award ceremony" },
              { src: "/media/lifestyle/game-face.jpg", alt: "Player focus" },
              { src: "/media/lifestyle/layup-contested.jpg", alt: "Contested layup" },
              { src: "/media/lifestyle/team-celebration.jpg", alt: "Team celebration" },
            ].map((img) => (
              <div
                key={img.src}
                className="tm-img-zoom relative aspect-square overflow-hidden"
              >
                <Image
                  src={img.src}
                  alt={img.alt}
                  fill
                  sizes="(max-width: 640px) 100vw, (max-width: 1024px) 50vw, 25vw"
                  className="object-cover"
                />
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CTA */}
      <section className="bg-tm-black py-20">
        <div className="tm-container text-center">
          <h2
            className="mb-6 text-3xl md:text-4xl font-semibold tracking-tight text-white"

          >
            Be Part of the Super6 Story
          </h2>
          <p className="mx-auto mb-8 max-w-lg text-sm text-white/60 leading-relaxed">
            Whether you&apos;re a coach, parent, or young athlete — there&apos;s
            a place for you in the Super6 community.
          </p>
          <Link href="/register" className="tm-btn tm-btn-white tm-btn-pill">
            Register Your Team
          </Link>
        </div>
      </section>
    </>
  );
}
