export interface ProjectWeatherReport {
  reportId: string;
  projectId: string;
  projectCode: string;
  projectName: string;
  reportDate: string;
  
  // Temperature
  currentTemperature: number;
  minTemperature: number;
  maxTemperature: number;
  
  // Humidity
  humidity: number;
  
  // Precipitation
  precipitationProbability: number;
  precipitationAmount: number;
  
  // Wind
  windSpeed: number;
  windGust: number;
  windDirection: number;
  
  // Pressure
  pressure: number;
  
  // Solar Radiation
  solarRadiation: number;
  
  // Sun and Moon
  sunrise?: string;
  sunset?: string;
  moonrise?: string;
  moonset?: string;
  
  // Location
  latitude: number;
  longitude: number;
  elevation: number;
  
  description: string;
  isFavorable: boolean;
  alertMessage?: string;
  qualityIssueCreated: boolean;
  createdDate: string;
}
