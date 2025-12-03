import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { VehicleReportHttpService } from './services/vehicle-report-http.service';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-report-vehicle',
  templateUrl: './report-vehicle.component.html',
  styleUrls: ['./report-vehicle.component.scss'],
  providers: [MessageService]
})
export class ReportVehicleComponent implements OnInit {
  
  isBusy: boolean;
  isInitializing: boolean;

  vehicleReport: any;

  costData: any;
  costOptions: any;
  incomeData: any;
  incomeOptions: any;
  progressData: any;
  progressOptions: any;


  constructor(private messageService: MessageService,
      private route: ActivatedRoute,
      private router: Router,
      private vehicleReportHttpService: VehicleReportHttpService) {

    this.isBusy = false;
    this.isInitializing = false;

  }

  ngOnInit(): void {

    this.isBusy = true;
    this.route.paramMap.pipe(
      switchMap(params => {

        const id = parseInt(params.get('id')!, 10);
        return this.vehicleReportHttpService.getReport(id);
      })
    )
    .subscribe({
      next: (successResponse) => {
        
        const temp = successResponse as any;
        this.vehicleReport = {
          id: temp.id,
          data: JSON.parse(temp.data)
        };

        this.generateCostGraph(this.vehicleReport.data.lastCostDetails);
        this.generateIncomeGraph(this.vehicleReport.data.lastIncomeDetails);
        this.generateProgressGraph(this.vehicleReport.data.progressDetails);

        this.isBusy = false;
      },
      error: (errorResponse) => {
        this.isBusy = false;
      }
    });
  }


  public onClickBack(): void {
    this.router.navigate(['/features/fleet/vehicle']);
  }

  public onClickRefresh(): void {

    this.isBusy = true;
    this.vehicleReportHttpService.generateReport(this.vehicleReport.id)
      .subscribe({
        next: (successResponse) => {

          this.isBusy = false;
          window.location.reload();
        },
        error: (errorResponse) => {

          this.isBusy = false;
          this.messageService.add({
            detail: 'Failed to refresh report',
            summary: 'Error',
            severity: 'error'
          });
        }
      });
  }

  public generateCostGraph(data: any): void {
    
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = documentStyle.getPropertyValue('--text-color');
    const textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
    const surfaceBorder = documentStyle.getPropertyValue('--surface-border');
    
    this.costData = {
        labels: data.map((d: any) => d.Date),
        datasets: [
            {
                label: 'Cost',
                backgroundColor: documentStyle.getPropertyValue('--pink-500'),
                borderColor: documentStyle.getPropertyValue('--pink-500'),
                data: data.map((d: any) => d.Amount)
            }
        ]
    };

    this.costOptions = {
        maintainAspectRatio: false,
        aspectRatio: 0.8,
        plugins: {
            legend: {
                labels: {
                    color: textColor
                }
            }
        },
        scales: {
            x: {
                ticks: {
                    color: textColorSecondary,
                    font: {
                        weight: 500
                    }
                },
                grid: {
                    color: surfaceBorder,
                    drawBorder: false
                }
            },
            y: {
                ticks: {
                    color: textColorSecondary
                },
                grid: {
                    color: surfaceBorder,
                    drawBorder: false
                }
            }

        }
    };
  }

  public generateIncomeGraph(data: any): void {

    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = documentStyle.getPropertyValue('--text-color');
    const textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
    const surfaceBorder = documentStyle.getPropertyValue('--surface-border');
    
    this.incomeData = {
        labels: data.map((d: any) => d.Date),
        datasets: [
            {
                label: 'Income',
                backgroundColor: documentStyle.getPropertyValue('--blue-500'),
                borderColor: documentStyle.getPropertyValue('--blue-500'),
                data: data.map((d: any) => d.Amount)
            }
        ]
    };

    this.incomeOptions = {
        maintainAspectRatio: false,
        aspectRatio: 0.8,
        plugins: {
            legend: {
                labels: {
                    color: textColor
                }
            }
        },
        scales: {
            x: {
                ticks: {
                    color: textColorSecondary,
                    font: {
                        weight: 500
                    }
                },
                grid: {
                    color: surfaceBorder,
                    drawBorder: false
                }
            },
            y: {
                ticks: {
                    color: textColorSecondary
                },
                grid: {
                    color: surfaceBorder,
                    drawBorder: false
                }
            }

        }
    };
  }

  public generateProgressGraph(data: any): void {

    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = documentStyle.getPropertyValue('--text-color');
    const textColorSecondary = documentStyle.getPropertyValue('--text-color-secondary');
    const surfaceBorder = documentStyle.getPropertyValue('--surface-border');
    
    this.progressData = {
        labels: data.map((d: any) => d.Date),
        datasets: [
          {
            type: 'line',
            label: 'Total income',
            backgroundColor: documentStyle.getPropertyValue('--blue-500'),
            borderColor: documentStyle.getPropertyValue('--blue-500'),
            data: data.map((d: any) => d.TotalIncome)
          },
          {
            type: 'line',
              label: 'Total cost',
              backgroundColor: documentStyle.getPropertyValue('--pink-500'),
              borderColor: documentStyle.getPropertyValue('--pink-500'),
              data: data.map((d: any) => d.TotalCost)
          },
          {
            type: 'line',
              label: 'Maintanance cost',
              backgroundColor: documentStyle.getPropertyValue('--yellow-500'),
              borderColor: documentStyle.getPropertyValue('--yellow-500'),
              data: data.map((d: any) => d.MaintananceCost)
          }
        ]
    };

    this.progressOptions = {
        maintainAspectRatio: false,
        aspectRatio: 0.8,
        plugins: {
            legend: {
                labels: {
                    color: textColor
                }
            }
        },
        scales: {
            x: {
                ticks: {
                    color: textColorSecondary,
                    font: {
                        weight: 500
                    }
                },
                grid: {
                    color: surfaceBorder,
                    drawBorder: false
                }
            },
            y: {
                ticks: {
                    color: textColorSecondary
                },
                grid: {
                    color: surfaceBorder,
                    drawBorder: false
                }
            }

        }
    };
  }
}
