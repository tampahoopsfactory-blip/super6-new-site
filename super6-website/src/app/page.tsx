import HeroSection from "@/components/modules/HeroTileGrid";
import ImpactStrip from "@/components/modules/ServiceGrid";
import MissionSplit from "@/components/modules/SplitSections";
import PhotoGallery from "@/components/modules/FeatureBanner";
import ExperienceSection from "@/components/modules/StoryCardGrid";
import DivisionCards from "@/components/modules/LocationsTeaser";
import PricingSection from "@/components/modules/RegistrationCTA";
import LocationsGrid from "@/components/modules/TeamCarousel";
import CtaSection from "@/components/modules/CtaBanner";
import FadeIn from "@/components/ui/FadeIn";

/*
  Homepage — GC Ventures Dark Design
  Bold 600-weight typography × pure black backgrounds ×
  #F35422 accent × motion-forward. Photography-first layouts.
  Inspired by DD.NYC / gcventures.vc. Fade-in on scroll.
*/
export default function HomePage() {
  return (
    <>
      <HeroSection />
      <FadeIn><ImpactStrip /></FadeIn>
      <FadeIn><MissionSplit /></FadeIn>
      <FadeIn><PhotoGallery /></FadeIn>
      <FadeIn><ExperienceSection /></FadeIn>
      <FadeIn><DivisionCards /></FadeIn>
      <FadeIn><PricingSection /></FadeIn>
      <FadeIn><LocationsGrid /></FadeIn>
      <FadeIn><CtaSection /></FadeIn>
    </>
  );
}
