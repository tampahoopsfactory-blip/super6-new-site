"use client";

import {
  Sparkles,
  Smartphone,
  Calendar,
  BookOpen,
  ShieldCheck,
  Users,
  MapPin,
  CloudRain,
  type LucideIcon,
} from "lucide-react";
import type { FaqIcon } from "../faq-data";

/* ─── Maps the FaqIcon string in faq-data.ts → Lucide component.
   Single source of truth so the data file stays serializable & free of
   React imports. */

const REGISTRY: Record<FaqIcon, LucideIcon> = {
  sparkles: Sparkles,
  smartphone: Smartphone,
  calendar: Calendar,
  "book-open": BookOpen,
  "shield-check": ShieldCheck,
  users: Users,
  "map-pin": MapPin,
  "cloud-rain": CloudRain,
};

type Props = {
  name: FaqIcon;
  size?: number;
  strokeWidth?: number;
  className?: string;
};

export default function SectionIcon({
  name,
  size = 18,
  strokeWidth = 2,
  className,
}: Props) {
  const Icon = REGISTRY[name] ?? Sparkles;
  return (
    <Icon size={size} strokeWidth={strokeWidth} className={className} aria-hidden="true" />
  );
}
