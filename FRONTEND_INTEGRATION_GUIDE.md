# Frontend Integration Guide for QA/QC Workspace Filters

## Overview
This guide shows how to integrate the new dropdown filter endpoints in your frontend application.

## API Endpoints

### 1. Get WIR Checkpoint Filters
```
GET /api/wircheckpoints/filters
```

### 2. Get Quality Issue Filters
```
GET /api/qualityissues/filters
```

## Integration Examples

### React/TypeScript Example

#### 1. Define Types

```typescript
// types/filters.ts
interface WIRCheckpointFilters {
  stageNumbers: string[];
  boxTags: string[];
  projectCodes: string[];
}

interface QualityIssueFilters {
  issueNumbers: string[];
  boxTags: string[];
}

interface ApiResponse<T> {
  isSuccess: boolean;
  data: T;
  message?: string;
}
```

#### 2. Create API Service

```typescript
// services/qaqcService.ts
import axios from 'axios';

const API_BASE_URL = 'https://your-api-url/api';

export const qaqcService = {
  // Fetch WIR Checkpoint filters
  async getWIRCheckpointFilters(): Promise<WIRCheckpointFilters> {
    const response = await axios.get<ApiResponse<WIRCheckpointFilters>>(
      `${API_BASE_URL}/wircheckpoints/filters`
    );
    return response.data.data;
  },

  // Fetch Quality Issue filters
  async getQualityIssueFilters(): Promise<QualityIssueFilters> {
    const response = await axios.get<ApiResponse<QualityIssueFilters>>(
      `${API_BASE_URL}/qualityissues/filters`
    );
    return response.data.data;
  }
};
```

#### 3. Use in Component - WIR Checkpoints Section

```typescript
// components/QAQCWorkspace/CheckpointsFilters.tsx
import React, { useEffect, useState } from 'react';
import { qaqcService } from '../../services/qaqcService';

interface CheckpointsFiltersProps {
  onFilterChange: (filters: any) => void;
}

const CheckpointsFilters: React.FC<CheckpointsFiltersProps> = ({ onFilterChange }) => {
  const [filters, setFilters] = useState<WIRCheckpointFilters | null>(null);
  const [loading, setLoading] = useState(true);
  
  const [selectedStageNumber, setSelectedStageNumber] = useState('');
  const [selectedBoxTag, setSelectedBoxTag] = useState('');
  const [selectedProjectCode, setSelectedProjectCode] = useState('');

  useEffect(() => {
    loadFilters();
  }, []);

  const loadFilters = async () => {
    try {
      setLoading(true);
      const data = await qaqcService.getWIRCheckpointFilters();
      setFilters(data);
    } catch (error) {
      console.error('Failed to load filters:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleStageNumberChange = (value: string) => {
    setSelectedStageNumber(value);
    onFilterChange({ 
      stageNumber: value, 
      boxTag: selectedBoxTag, 
      projectCode: selectedProjectCode 
    });
  };

  const handleBoxTagChange = (value: string) => {
    setSelectedBoxTag(value);
    onFilterChange({ 
      stageNumber: selectedStageNumber, 
      boxTag: value, 
      projectCode: selectedProjectCode 
    });
  };

  const handleProjectCodeChange = (value: string) => {
    setSelectedProjectCode(value);
    onFilterChange({ 
      stageNumber: selectedStageNumber, 
      boxTag: selectedBoxTag, 
      projectCode: value 
    });
  };

  if (loading) return <div>Loading filters...</div>;

  return (
    <div className="filters-container">
      {/* Stage Number Dropdown */}
      <div className="filter-group">
        <label>Stage Number</label>
        <select 
          value={selectedStageNumber} 
          onChange={(e) => handleStageNumberChange(e.target.value)}
        >
          <option value="">All</option>
          {filters?.stageNumbers.map(stage => (
            <option key={stage} value={stage}>{stage}</option>
          ))}
        </select>
      </div>

      {/* Box Tag Dropdown */}
      <div className="filter-group">
        <label>Box Tag</label>
        <select 
          value={selectedBoxTag} 
          onChange={(e) => handleBoxTagChange(e.target.value)}
        >
          <option value="">All</option>
          {filters?.boxTags.map(tag => (
            <option key={tag} value={tag}>{tag}</option>
          ))}
        </select>
      </div>

      {/* Project Code Dropdown */}
      <div className="filter-group">
        <label>Project Code</label>
        <select 
          value={selectedProjectCode} 
          onChange={(e) => handleProjectCodeChange(e.target.value)}
        >
          <option value="">All</option>
          {filters?.projectCodes.map(code => (
            <option key={code} value={code}>{code}</option>
          ))}
        </select>
      </div>
    </div>
  );
};

export default CheckpointsFilters;
```

#### 4. Use in Component - Quality Issues Section

```typescript
// components/QAQCWorkspace/QualityIssuesFilters.tsx
import React, { useEffect, useState } from 'react';
import { qaqcService } from '../../services/qaqcService';

interface QualityIssuesFiltersProps {
  onFilterChange: (filters: any) => void;
}

const QualityIssuesFilters: React.FC<QualityIssuesFiltersProps> = ({ onFilterChange }) => {
  const [filters, setFilters] = useState<QualityIssueFilters | null>(null);
  const [loading, setLoading] = useState(true);
  
  const [selectedIssueNumber, setSelectedIssueNumber] = useState('');
  const [selectedBoxTag, setSelectedBoxTag] = useState('');

  useEffect(() => {
    loadFilters();
  }, []);

  const loadFilters = async () => {
    try {
      setLoading(true);
      const data = await qaqcService.getQualityIssueFilters();
      setFilters(data);
    } catch (error) {
      console.error('Failed to load filters:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleIssueNumberChange = (value: string) => {
    setSelectedIssueNumber(value);
    onFilterChange({ issueNumber: value, boxTag: selectedBoxTag });
  };

  const handleBoxTagChange = (value: string) => {
    setSelectedBoxTag(value);
    onFilterChange({ issueNumber: selectedIssueNumber, boxTag: value });
  };

  if (loading) return <div>Loading filters...</div>;

  return (
    <div className="filters-container">
      {/* Issue Number Dropdown */}
      <div className="filter-group">
        <label>Issue Number</label>
        <select 
          value={selectedIssueNumber} 
          onChange={(e) => handleIssueNumberChange(e.target.value)}
        >
          <option value="">All</option>
          {filters?.issueNumbers.map(issue => (
            <option key={issue} value={issue}>{issue}</option>
          ))}
        </select>
      </div>

      {/* Box Tag Dropdown */}
      <div className="filter-group">
        <label>Box Tag</label>
        <select 
          value={selectedBoxTag} 
          onChange={(e) => handleBoxTagChange(e.target.value)}
        >
          <option value="">All</option>
          {filters?.boxTags.map(tag => (
            <option key={tag} value={tag}>{tag}</option>
          ))}
        </select>
      </div>
    </div>
  );
};

export default QualityIssuesFilters;
```

### Angular Example

```typescript
// services/qaqc.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

interface ApiResponse<T> {
  isSuccess: boolean;
  data: T;
  message?: string;
}

interface WIRCheckpointFilters {
  stageNumbers: string[];
  boxTags: string[];
  projectCodes: string[];
}

interface QualityIssueFilters {
  issueNumbers: string[];
  boxTags: string[];
}

@Injectable({
  providedIn: 'root'
})
export class QaqcService {
  private baseUrl = 'https://your-api-url/api';

  constructor(private http: HttpClient) {}

  getWIRCheckpointFilters(): Observable<WIRCheckpointFilters> {
    return this.http
      .get<ApiResponse<WIRCheckpointFilters>>(`${this.baseUrl}/wircheckpoints/filters`)
      .pipe(map(response => response.data));
  }

  getQualityIssueFilters(): Observable<QualityIssueFilters> {
    return this.http
      .get<ApiResponse<QualityIssueFilters>>(`${this.baseUrl}/qualityissues/filters`)
      .pipe(map(response => response.data));
  }
}

// components/checkpoints-filters/checkpoints-filters.component.ts
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { QaqcService } from '../../services/qaqc.service';

@Component({
  selector: 'app-checkpoints-filters',
  templateUrl: './checkpoints-filters.component.html'
})
export class CheckpointsFiltersComponent implements OnInit {
  @Output() filterChange = new EventEmitter<any>();

  filters: WIRCheckpointFilters | null = null;
  loading = true;

  selectedStageNumber = '';
  selectedBoxTag = '';
  selectedProjectCode = '';

  constructor(private qaqcService: QaqcService) {}

  ngOnInit() {
    this.loadFilters();
  }

  loadFilters() {
    this.qaqcService.getWIRCheckpointFilters().subscribe({
      next: (data) => {
        this.filters = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Failed to load filters:', error);
        this.loading = false;
      }
    });
  }

  onStageNumberChange(value: string) {
    this.selectedStageNumber = value;
    this.emitFilterChange();
  }

  onBoxTagChange(value: string) {
    this.selectedBoxTag = value;
    this.emitFilterChange();
  }

  onProjectCodeChange(value: string) {
    this.selectedProjectCode = value;
    this.emitFilterChange();
  }

  private emitFilterChange() {
    this.filterChange.emit({
      stageNumber: this.selectedStageNumber,
      boxTag: this.selectedBoxTag,
      projectCode: this.selectedProjectCode
    });
  }
}
```

### Vue.js Example

```typescript
// composables/useQaqcFilters.ts
import { ref, onMounted } from 'vue';
import axios from 'axios';

interface WIRCheckpointFilters {
  stageNumbers: string[];
  boxTags: string[];
  projectCodes: string[];
}

interface QualityIssueFilters {
  issueNumbers: string[];
  boxTags: string[];
}

export function useWIRCheckpointFilters() {
  const filters = ref<WIRCheckpointFilters | null>(null);
  const loading = ref(true);
  const error = ref<string | null>(null);

  const loadFilters = async () => {
    try {
      loading.value = true;
      const response = await axios.get('/api/wircheckpoints/filters');
      filters.value = response.data.data;
    } catch (err) {
      error.value = 'Failed to load filters';
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  onMounted(() => {
    loadFilters();
  });

  return { filters, loading, error, loadFilters };
}

export function useQualityIssueFilters() {
  const filters = ref<QualityIssueFilters | null>(null);
  const loading = ref(true);
  const error = ref<string | null>(null);

  const loadFilters = async () => {
    try {
      loading.value = true;
      const response = await axios.get('/api/qualityissues/filters');
      filters.value = response.data.data;
    } catch (err) {
      error.value = 'Failed to load filters';
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  onMounted(() => {
    loadFilters();
  });

  return { filters, loading, error, loadFilters };
}

// components/CheckpointsFilters.vue
<template>
  <div class="filters-container">
    <div v-if="loading">Loading filters...</div>
    <div v-else-if="error">{{ error }}</div>
    <div v-else>
      <div class="filter-group">
        <label>Stage Number</label>
        <select v-model="selectedStageNumber" @change="handleFilterChange">
          <option value="">All</option>
          <option v-for="stage in filters?.stageNumbers" :key="stage" :value="stage">
            {{ stage }}
          </option>
        </select>
      </div>

      <div class="filter-group">
        <label>Box Tag</label>
        <select v-model="selectedBoxTag" @change="handleFilterChange">
          <option value="">All</option>
          <option v-for="tag in filters?.boxTags" :key="tag" :value="tag">
            {{ tag }}
          </option>
        </select>
      </div>

      <div class="filter-group">
        <label>Project Code</label>
        <select v-model="selectedProjectCode" @change="handleFilterChange">
          <option value="">All</option>
          <option v-for="code in filters?.projectCodes" :key="code" :value="code">
            {{ code }}
          </option>
        </select>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useWIRCheckpointFilters } from '@/composables/useQaqcFilters';

const emit = defineEmits<{
  filterChange: [filters: any]
}>();

const { filters, loading, error } = useWIRCheckpointFilters();

const selectedStageNumber = ref('');
const selectedBoxTag = ref('');
const selectedProjectCode = ref('');

const handleFilterChange = () => {
  emit('filterChange', {
    stageNumber: selectedStageNumber.value,
    boxTag: selectedBoxTag.value,
    projectCode: selectedProjectCode.value
  });
};
</script>
```

## Best Practices

1. **Caching**: Consider caching filter results since they don't change frequently
2. **Error Handling**: Always handle API errors gracefully
3. **Loading States**: Show loading indicators while fetching filters
4. **Debouncing**: If using search/autocomplete dropdowns, debounce user input
5. **Reset Filters**: Provide a way to reset all filters to default
6. **URL State**: Consider storing filter state in URL query parameters for bookmarking

## Notes

- Filter endpoints return all available values, not paginated
- Values are pre-sorted alphabetically by the backend
- Empty/null values are filtered out
- Filters respect user permissions (only shows data from accessible projects)
