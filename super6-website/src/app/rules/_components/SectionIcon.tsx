"use client";

import {
  Scale,
  Shirt,
  ClipboardList,
  Timer,
  Trophy,
  UserCheck,
  CalendarClock,
  type LucideIcon,
} from "lucide-react";
import type { RuleIcon } from "../rules-data";

/* ─── Maps the RuleIcon string in rules-data.ts → Lucide component.
   Mirrors src/app/faq/_components/SectionIcon.tsx; separate registry so
   rules-data.ts stays free of React imports and the icon set can evolve
   independently from the FAQ icon set. */

const REGISTRY: Record<RuleIcon, LucideIcon> = {
  scale: Scale,
  shirt: Shirt,
  "clipboard-list": ClipboardList,
  timer: Timer,
  trophy: Trophy,
  "user-check": UserCheck,
  "calendar-clock": CalendarClock,
};

type Props = {
  name: RuleIcon;
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
  const Icon = REGISTRY[name] ?? Scale;
  return (
    <Icon
      size={size}
      strokeWidth={strokeWidth}
      className={className}
      aria-hidden="true"
    />
  );
}
