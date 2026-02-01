import { calculateAndFormatDuration, calculateDurationValues, calculateDurationInDays } from './duration.util';

describe('Duration Utility Functions', () => {
  describe('calculateAndFormatDuration', () => {
    it('should format duration less than 1 hour in minutes', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-01T10:30:00');
      const result = calculateAndFormatDuration(start, end);
      expect(result).toBe('30 minutes');
    });

    it('should format exactly 1 minute', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-01T10:01:00');
      const result = calculateAndFormatDuration(start, end);
      expect(result).toBe('1 minute');
    });

    it('should format duration less than 24 hours in hours', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-01T15:30:00');
      const result = calculateAndFormatDuration(start, end);
      expect(result).toBe('5.5 hours');
    });

    it('should format exactly 1 hour', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-01T11:00:00');
      const result = calculateAndFormatDuration(start, end);
      expect(result).toBe('1 hour');
    });

    it('should format exactly 24 hours as 1 day', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-02T10:00:00');
      const result = calculateAndFormatDuration(start, end);
      expect(result).toBe('1 day');
    });

    it('should format duration with days and hours', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-03T15:00:00');
      const result = calculateAndFormatDuration(start, end);
      expect(result).toBe('2 days 5 hours');
    });

    it('should format duration with days and 1 hour', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-03T11:00:00');
      const result = calculateAndFormatDuration(start, end);
      expect(result).toBe('2 days 1 hour');
    });

    it('should format duration with exact days (no remaining hours)', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-04T10:00:00');
      const result = calculateAndFormatDuration(start, end);
      expect(result).toBe('3 days');
    });

    it('should handle string date inputs', () => {
      const result = calculateAndFormatDuration(
        '2024-01-01T10:00:00',
        '2024-01-01T15:30:00'
      );
      expect(result).toBe('5.5 hours');
    });

    it('should return undefined for invalid dates', () => {
      const result = calculateAndFormatDuration('invalid', 'dates');
      expect(result).toBeUndefined();
    });

    it('should return undefined for missing dates', () => {
      const result = calculateAndFormatDuration(undefined, undefined);
      expect(result).toBeUndefined();
    });

    it('should return undefined for negative duration', () => {
      const start = new Date('2024-01-02T10:00:00');
      const end = new Date('2024-01-01T10:00:00');
      const result = calculateAndFormatDuration(start, end);
      expect(result).toBeUndefined();
    });
  });

  describe('calculateDurationValues', () => {
    it('should return correct duration values', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-03T15:00:00');
      const result = calculateDurationValues(start, end);
      
      expect(result).toBeDefined();
      expect(result?.days).toBe(2);
      expect(result?.hours).toBe(5);
      expect(result?.totalHours).toBe(53);
    });

    it('should return undefined for invalid inputs', () => {
      const result = calculateDurationValues(undefined, undefined);
      expect(result).toBeUndefined();
    });
  });

  describe('calculateDurationInDays (legacy)', () => {
    it('should return 1 for same calendar day', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-01T15:00:00');
      const result = calculateDurationInDays(start, end);
      expect(result).toBe(1);
    });

    it('should calculate calendar days + 1 for different days', () => {
      const start = new Date('2024-01-01T10:00:00');
      const end = new Date('2024-01-03T15:00:00');
      const result = calculateDurationInDays(start, end);
      expect(result).toBe(3); // 2 days difference + 1 = 3
    });

    it('should return undefined for invalid dates', () => {
      const result = calculateDurationInDays('invalid', 'dates');
      expect(result).toBeUndefined();
    });
  });
});

