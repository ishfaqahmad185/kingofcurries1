(function($) {
    /* "use strict" */


 var dzChartlist = function(){
	
	var screenWidth = $(window).width();
		
	
	
	var donutChart2 = function(){
		var options = {
			colors:['#ff5c5a', '#2bc156', '#404a56'],
			chart: {
				width: 140,
				height: 140,
				type: 'donut',
				sparkline: {
					enabled: true,
				},
				
			},
			plotOptions: {
				pie: {
					customScale: 1,
					donut: {
						size: '50%'
						
					}
				}
			},
			dataLabels: {
				enabled: false
			},
			responsive: [{
				breakpoint: 1300,
				options: {
					chart: {
						width: 120,
						height: 120
					},
				}
			}],
			legend: {
				show: false
			}
        };
        var chart = new ApexCharts(document.querySelector("#chart2"), options);
        chart.render();
	}
	var donutChart3 = function(){
		var options = {
			series: [35, 45, 20],
			colors:['#ff5c5a', '#2bc156', '#404a56'],
			chart: {
				width: 140,
				height: 140,
				type: 'donut',
				sparkline: {
					enabled: true,
				},
				
			},
			plotOptions: {
				pie: {
					customScale: 1,
					donut: {
						size: '50%'
						
					}
				}
			},
			dataLabels: {
				enabled: false
			},
			responsive: [{
				breakpoint: 1300,
				options: {
					chart: {
						width: 120,
						height: 120
					},
				}
			}],
			legend: {
				show: false
			}
        };
        var chart = new ApexCharts(document.querySelector("#chart3"), options);
        chart.render();
	}
	
	var chartBar = function(){
		
		var options = {
			  series: [
				{
					name: 'Net Profit',
					data: [31, 40, 28, 51, 42, 109, 100],
					//radius: 12,	
				}, 
				{
				  name: 'Revenue',
				  data: [11, 32, 45, 32, 34, 52, 41]
				}, 
				
			],
				chart: {
				type: 'area',
				height: 350,
				toolbar: {
					show: false,
				},
				
			},
			plotOptions: {
			  bar: {
				horizontal: false,
				columnWidth: '55%',
				endingShape: 'rounded'
			  },
			},
			colors:['#2f4cdd', '#b519ec'],
			dataLabels: {
			  enabled: false,
			},
			markers: {
		shape: "circle",
		},
			legend: {
				show: true,
				fontSize: '12px',
				
				labels: {
					colors: '#000000',
					
				},
				position: 'top',
				horizontalAlign: 'left', 	
				markers: {
					width: 19,
					height: 19,
					strokeWidth: 0,
					strokeColor: '#fff',
					fillColors: undefined,
					radius: 4,
					offsetX: -5,
					offsetY: -5	
				}
			},
			stroke: {
			  show: true,
			  width: 4,
			  colors:['#2f4cdd', '#b519ec'],
			},
			
			grid: {
				borderColor: '#eee',
			},
			xaxis: {
				
			  categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'July'],
			  labels: {
				style: {
					colors: '#3e4954',
					fontSize: '13px',
					fontFamily: 'Poppins',
					fontWeight: 100,
					cssClass: 'apexcharts-xaxis-label',
				},
			  },
			  crosshairs: {
			  show: false,
			  }
			},
			yaxis: {
				labels: {
			   style: {
				  colors: '#3e4954',
				  fontSize: '13px',
				   fontFamily: 'Poppins',
				  fontWeight: 100,
				  cssClass: 'apexcharts-xaxis-label',
			  },
			  },
			},
			fill: {
			  opacity: 1
			},
			tooltip: {
			  y: {
				formatter: function (val) {
				  return "$ " + val + " thousands"
				}
			  }
			}
			};

			var chartBar1 = new ApexCharts(document.querySelector("#chartBar"), options);
			chartBar1.render();
	}
	
	var counterBar = function(){
		$(".counter").counterUp({
			delay: 30,
			time: 3000
		});
	}
	
	
	/* Function ============ */
		return {
			init:function(){
			},
			
			
			load:function(){
					
					
				donutChart2();	
				donutChart3();	
				chartBar();
				counterBar();
			},
			
			resize:function(){
				
			}
		}
	
	}();

	jQuery(document).ready(function(){
	});
		
	jQuery(window).on('load',function(){
		setTimeout(function(){
			dzChartlist.load();
		}, 1000); 
		
	});

	jQuery(window).on('resize',function(){
		
		
	});     

})(jQuery);