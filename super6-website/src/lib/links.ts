/**
 * External registration link.
 * Exposure Events handles all Super6 Series LLC tournament scheduling and registration,
 * so every "Register your team" button across the site points here.
 */
export const REGISTER_URL =
  "https://basketball.exposureevents.com/store?storeorganizationid=14807#events";

/** Standard props applied to every external Register button/link. */
export const REGISTER_LINK_PROPS = {
  href: REGISTER_URL,
  target: "_blank" as const,
  rel: "noopener noreferrer",
};
