import HeroSection from "@/components/modules/HeroTileGrid";
import HeroActionBar from "@/components/modules/HeroActionBar";
import ImpactStrip from "@/components/modules/ServiceGrid";
import MissionSplit from "@/components/modules/SplitSections";
import PhotoGallery from "@/components/modules/FeatureBanner";
import AdvantagesSection from "@/components/modules/AdvantagesSection";
import ProgramsSection from "@/components/modules/ProgramsSection";
import TestimonialsSection from "@/components/modules/TestimonialsSection";
import DivisionCards from "@/components/modules/LocationsTeaser";
import PricingSection from "@/components/modules/RegistrationCTA";
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
      <HeroActionBar />
      <FadeIn><ImpactStrip /></FadeIn>
      <FadeIn><MissionSplit /></FadeIn>
      <FadeIn><AdvantagesSection /></FadeIn>
      <FadeIn><PhotoGallery /></FadeIn>
      <FadeIn><ProgramsSection /></FadeIn>
      <FadeIn><DivisionCards /></FadeIn>
      <FadeIn><PricingSection /></FadeIn>
      <FadeIn><TestimonialsSection /></FadeIn>
      <FadeIn><CtaSection /></FadeIn>
    </>
  );
}
