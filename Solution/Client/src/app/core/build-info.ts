// BUILD_TIME is filled in by scripts/ng.mjs (npm start / npm run build) through the Angular CLI's --define.
// A plain "ng build" or a test run leaves it undefined, so this falls back to null.
declare const BUILD_TIME: string | undefined;

export const buildTime: Date | null = typeof BUILD_TIME === 'string' ? new Date(BUILD_TIME) : null;
