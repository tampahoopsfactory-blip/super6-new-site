import Image from "next/image";
import SectionIcon from "../../rules/_components/SectionIcon";
import type { RuleIcon } from "../../rules/rules-data";

export type CoachesSplitSectionProps = {
  section: {
    id: string;
    number: string;
    title: string;
    description?: string;
    icon: RuleIcon;
    image?: {
      src: string;
      alt: string;
      cropLetterbox?: boolean;
    };
  };
  children: React.ReactNode;
  /** Photo in the left column on desktop (and stacked first on mobile). */
  imageFirst?: boolean;
  /** Use `contain` when the photo must not be cropped (letterboxing inside the frame). */
  imageObjectFit?: "cover" | "contain";
};

/* Full-bleed editorial split.
   Default: copy column left, photo right (DOM: content then image).
   Pass imageFirst to flip desktop columns (photo left, copy right) via
   grid item order — keeps one markup shape for LCP / hydration.

   Background tint alternates by section number parity:
     odd  (01, 03, 05) → base cream
     even (02, 04)     → cream-warm */
export default function CoachesSplit({
  section,
  children,
  imageFirst = false,
  imageObjectFit = "cover",
}: CoachesSplitSectionProps) {
  const isEven = Number.parseInt(section.number, 10) % 2 === 0;
  const className =
    "coaches-split" +
    (isEven ? " coaches-split--alt" : "") +
    (imageFirst ? " coaches-split--image-first" : "");
  const titleId = `coaches-section-${section.id}-title`;

  return (
    <section id={section.id} className={className} aria-labelledby={titleId}>
      <div className="coaches-split-grid">
        {/* Copy first so in the two-column grid it anchors left and the
            photo sits right (grid column order follows DOM order). */}
        <div className="coaches-split-content">
          <div className="coaches-split-content-inner">
            <header className="faq-section-header coaches-split-header">
              <span className="faq-section-watermark" aria-hidden="true">
                {section.number}
              </span>

              <div className="faq-section-icon">
                <SectionIcon name={section.icon} size={22} strokeWidth={1.8} />
              </div>

              <div className="faq-section-titlewrap">
                <p className="faq-section-eyebrow">
                  <span
                    className="faq-section-eyebrow-rule"
                    aria-hidden="true"
                  />
                  Section {section.number}
                </p>
                <h2 id={titleId} className="faq-section-title">
                  {section.title}
                </h2>
                {section.description ? (
                  <p className="faq-section-desc">{section.description}</p>
                ) : null}
              </div>
            </header>

            <div className="coaches-split-body">{children}</div>
          </div>
        </div>

        {section.image ? (
          <div
            className={
              "coaches-split-image" +
              (imageObjectFit === "contain"
                ? " coaches-split-image--contain"
                : "") +
              (section.image.cropLetterbox
                ? " coaches-split-image--crop-letterbox"
                : "")
            }
          >
            <Image
              src={section.image.src}
              alt={section.image.alt}
              fill
              quality={100}
              sizes="(max-width: 968px) 100vw, 50vw"
              style={{
                objectFit: imageObjectFit,
                objectPosition: section.image.cropLetterbox
                  ? "center 36%"
                  : "center",
              }}
            />
          </div>
        ) : null}
      </div>
    </section>
  );
}
