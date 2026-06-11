/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{js,jsx,ts,tsx}"],
  theme: {
    extend: {
      colors: {
        royalblue: '#4169E1',
        cyan: '#06B6D4',
        'cyan-600': '#0891B2',
      },
      backgroundImage: {
        'gradient-main': 'linear-gradient(135deg, #4169E1 0%, #06B6D4 100%)',
      }
    },
  },
  plugins: [],
}
