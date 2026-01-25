module.exports = {
  preset: 'jest-preset-angular',
  setupFilesAfterEnv: ['<rootDir>/setup-jest.ts'],
  testPathIgnorePatterns: [
    '<rootDir>/node_modules/',
    '<rootDir>/projects/no-days-off/src/e2e/'
  ],
  moduleNameMapper: {
    '^@app/(.*)$': '<rootDir>/projects/no-days-off/src/app/$1',
    '^@components/(.*)$': '<rootDir>/projects/no-days-off/src/app/components/$1',
    '^@pages/(.*)$': '<rootDir>/projects/no-days-off/src/app/pages/$1',
    '^@services/(.*)$': '<rootDir>/projects/no-days-off/src/app/services/$1'
  },
  transformIgnorePatterns: ['node_modules/(?!@angular|rxjs)'],
  testMatch: ['**/*.spec.ts'],
  moduleFileExtensions: ['ts', 'html', 'js', 'json'],
  collectCoverageFrom: [
    'projects/no-days-off/src/app/**/*.ts',
    '!projects/no-days-off/src/app/**/*.spec.ts',
    '!projects/no-days-off/src/app/**/index.ts',
    '!projects/no-days-off/src/e2e/**/*'
  ]
};
