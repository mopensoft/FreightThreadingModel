% Freight Train Scheduling Problem
% Date: June 16th, 2014

%included data file here for testing purpose
include "ft16.dzn";
include "loop_test1_mgw.dzn";

%annotation cpx_max_activity;
include "cumulative.mzn";
%include "cpx_zinc_search.mzn";

int: O = 1; int: D = 2; %O - Origin, D - Destination
int: LINKDURATION = 4; %Fourth column of rail_links array gives the time taken by a passenger train to travel the corresponding link.
int: crosslinks = 10; % Total number of cross links
int: plinks = 4 ; % Passenger train path length
int: nfreight_trains; %No of freight trains to be scheduled in a day.
int: maxstops; % Maximum no. of stops that a freight train is allowed to take, including both originating and destination station
int: nlinks; %Total no. of railway links available in the given rail network
int: time_gap; %Minimum time gap between trains
% nstations - not used
int: nstations; %Total no. of railway station available in the given rail network
%  Speed factor assumed to be 1 in this model
int: npassenger_trains; %Total no. of passenger train that runs in a day in the given railway network.

set of int: FreightTrains = 1..nfreight_trains;
set of int: PassengerTrains = 1..npassenger_trains;
set of int: RailLinks = 1..nlinks;
set of int: Stations = 1..nstations;
set of int: MaxStops = 1..maxstops;
set of int: CrossLinks = 1..crosslinks;

array [FreightTrains] of int: speed_factor; %Ratio between passenger train speed and freight train speed.
array[RailLinks,1..4] of int: rail_links1; %Contains all rail links, their end stands and the travel duration in that link.
array[0..nlinks,1..4] of int: rail_links = array2d(0..nlinks,1..4,[if i=0 then 0 else rail_links1[i,j] endif | i in 0..nlinks, j in 1..4]);
constraint assert((forall (i in RailLinks)(rail_links[i,1] in RailLinks)), "Invalid data file" ++ "Error in number of rail links");
constraint assert((forall (i in RailLinks, j in 2..3)(rail_links[i,j] in Stations)), "Invalid data file" ++ "Rail link connects non-existent stations");
constraint assert((forall (i in RailLinks)(rail_links[i,4] >= 0)), "Invalid data file" ++ "Rail link duration cannot be negative");

array[PassengerTrains,1..plinks] of int: passenger_train_path; %Contains all passenger trains and their travel link in sequential order.
constraint assert((forall (i in PassengerTrains, j in 1..plinks)(passenger_train_path[i,j] in RailLinks)), "Invalid data file" ++ "Passenger train path includes non-existent links");

array[PassengerTrains,1..plinks] of int: passenger_train_dtime; %Contains all passenger trains departure time at relevant link.
constraint assert((forall (i in PassengerTrains, j in 1..plinks)(passenger_train_dtime[i,j] >= 0)), "Invalid data file" ++ "Passenger train departure times includes negative time");

array[FreightTrains,O..D] of int: freight_trains; %Originating and destation stations of the Freight Trains need to be scheduled. eg freight_trains[.,O] for originating station
constraint assert((forall (i in FreightTrains, j in O..D)(freight_trains[i,j] in Stations)), "Invalid data file" ++ "Freight train connects non-existent stations");
array[1..5,1..2] of int: coupled_links; % Rail links that have same originating stations
array[CrossLinks,1..4] of int: coupled_links_four; % Rail links that have same originating stations

array[PassengerTrains,RailLinks] of int: PTT = array2d(PassengerTrains,RailLinks, 
                                                [sum(j in 1..plinks)(bool2int(r = passenger_train_path[i,j])*passenger_train_dtime[i,j]) | i in PassengerTrains, r in RailLinks] );


array[FreightTrains,MaxStops] of var 0..nlinks: freight_train_path; %.[.,L] - Lth link. frieght train path in sequential order.
array[FreightTrains,MaxStops] of var 0..1440: freight_train_dtime; %.[.,L] - Departure time at Lth link.
%constraint trace("all_diff\n", true);
%array[FreightTrains,FreightTrains] of var bool: order;
array[FreightTrains] of var 1..maxstops: stopcount;
array[FreightTrains,1..nlinks] of var bool: freight_train_sx;

%Constraint 8: Daily operational times of freight trains 
array[FreightTrains,1..nlinks] of var 0..1440: freight_train_lb;
array[FreightTrains,1..nlinks] of var 0..1440: freight_train_ub;
array[FreightTrains,1..nlinks] of var 0..1440: end_lb;
array[FreightTrains,1..nlinks] of var 0..1440: end_ub;


%Constraint 2: Start location of freight trains
constraint forall(i in FreightTrains)
             (rail_links[freight_train_path[i,1],2] = freight_trains[i,1]) ;

%Constraint 3 & 1: End location of freight trains & Length of Path
constraint forall(i in FreightTrains)
             (rail_links[freight_train_path[i,stopcount[i]],3] = freight_trains[i,2]) ;

%Constraint 4: Preceding railway links
constraint forall(i in FreightTrains, j in 1..maxstops-1)
             (stopcount[i]>j -> 
                 rail_links[freight_train_path[i,j],3] = rail_links[freight_train_path[i,j+1],2]) ;


%Constraint 1b: Set After stopping the remaining freight train links are set to 0
constraint forall(i in FreightTrains,j in MaxStops)
            ( j > stopcount[i] -> freight_train_path[i,j] = 0) ;




%Constraint 6: Freight trains that use the same railway link have a precedence between them
%Set true for the paths that are been used
constraint
    forall(i in FreightTrains, j in MaxStops)
            (j <= stopcount[i] -> freight_train_sx[i,freight_train_path[i,j]]);

%Set false for the paths that not been used
constraint
  forall(r in RailLinks, i in FreightTrains)
        ( forall(j in MaxStops) (freight_train_path[i,j] != r) -> freight_train_sx[i,r] = false);
%constraint
%    forall(i1,i2 in FreightTrains, r in RailLinks where i2!=i1)
%          ((freight_train_sx[i1,r]) /\ (freight_train_sx[i2,r])
%              ->        (order[i1,i2] \/ order[i2,i1]) );
%constraint
%    forall(i1,i2 in FreightTrains where i1!=i2)
%          (sum(r in RailLinks)(bool2int(freight_train_sx[i1,r] /\ freight_train_sx[i2,r] )) = 0 
%            ->
%                (order[i1,i2] = false /\ order[i2,i1] = false ) );

%For a pair of trains both precedence boolean cannot be true at the same time 
%constraint forall(i1,i2 in FreightTrains where i2>i1) 
%             (not (order[i1,i2] /\ order[i2,i1]) ) ;

%Order of the freight trains remains the same throughout the journey
%constraint forall(i1,i2,i3 in FreightTrains)
%             (order[i1,i2] /\ order[i2,i3] -> order[i1,i3]);


    
%Side Constraint 1: lines crossing constraint - if two train paths cross, then they must be separated by a headway
constraint forall(i1,i2 in FreightTrains, p in CrossLinks where i1!=i2)
                  (( (freight_train_sx[i1,coupled_links_four[p,1]]  /\ freight_train_sx[i1,coupled_links_four[p,4]])
                            /\ (freight_train_sx[i2,coupled_links_four[p,2]]  /\ freight_train_sx[i2,coupled_links_four[p,3]])
                    )
                       -> 
                        (freight_train_lb[i1,coupled_links_four[p,1]] >= freight_train_ub[i2,coupled_links_four[p,2]] + time_gap) 
                          \/
                        (freight_train_ub[i1,coupled_links_four[p,1]] <= freight_train_lb[i2,coupled_links_four[p,2]] - time_gap) );


%Constraint 7: Minimum headway time between trains is Gap
%freight-freight
%constraint
%    forall(i1,i2 in FreightTrains, r in RailLinks where i1!=i2)
%          ((freight_train_sx[i1,r] ) /\ (freight_train_sx[i2,r] /\ order[i2,i1])
%              ->
%            (freight_train_lb[i1,r] >= freight_train_ub[i2,r] + time_gap) );                             

%Side Constraint 3: Cyclic network constraint - a constraint precluding freight trains from taking paths with cycles
constraint forall(f in FreightTrains)(alldifferent_except_0([rail_links[freight_train_path[f,j],3]| j in MaxStops]));


%Side Constraint 2: No stopping - a freight train cannot stop en route
bool: canstop = true; 

constraint if canstop then true else
    (forall(f in FreightTrains, r1, r2 in RailLinks where rail_links[r1,3]=rail_links[r2,2])
    (end_lb[f,r1] >= freight_train_ub[f,r2])) endif;

%%  Line segment duration constraint
constraint forall(f in FreightTrains, r in RailLinks)
        (end_lb[f,r] >= freight_train_ub[f,r]+rail_links[r,LINKDURATION]*speed_factor[f]);
constraint
forall(f in FreightTrains, r1, r2 in RailLinks where rail_links[r1,3]=rail_links[r2,2])
    ( end_ub[f,r1] <= freight_train_lb[f,r2]);

%==========Cumulative start============================
%Cumulative constraint

include "globals.mzn";

%--------------Opturion-cpx - start--------------------

constraint forall(r in RailLinks)(cumulative_optional(
        [ freight_train_lb[f, r] | f in FreightTrains ] ++ [ PTT[p, r] | p in PassengerTrains ],
        [ freight_train_ub[f, r] | f in FreightTrains ] ++ [ PTT[p, r] | p in PassengerTrains ],
        [ freight_train_sx[f, r] | f in FreightTrains ] ++ [ PTT[p, r] > 0 | p in PassengerTrains ],
        [ time_gap | f in FreightTrains ] ++ [ time_gap | p in PassengerTrains ],
        [ 1 | f in FreightTrains ] ++ [ if PTT[p, r] > 0 then 1 else 0 endif | p in PassengerTrains ],
        1
    ));% :: ttef);

constraint forall(r in RailLinks)(cumulative_optional(
        [ end_lb[f, r] | f in FreightTrains ] ++ [ PTT[p, r]+rail_links[r,LINKDURATION] | p in PassengerTrains ],
        [ end_ub[f, r] | f in FreightTrains ] ++ [ PTT[p, r]+rail_links[r,LINKDURATION] | p in PassengerTrains ],
        [ freight_train_sx[f, r] | f in FreightTrains ] ++ [ PTT[p, r] > 0 | p in PassengerTrains ],
        [ time_gap | f in FreightTrains ] ++ [ time_gap | p in PassengerTrains ],
        [ 1 | f in FreightTrains ] ++ [ if PTT[p, r] > 0 then 1 else 0 endif | p in PassengerTrains ],
        1
    ));% :: ttef);


%--------------Opturion-cpx - end--------------------


%--------------Gecode - start--------------------
/*
include "gecode.mzn";

constraint forall(r in RailLinks)(gecode_schedule_cumulative_optional(
        [ freight_train_lb[f, r] | f in FreightTrains ] ++ [ PTT[p, r] | p in PassengerTrains ],
        [ time_gap | f in FreightTrains ] ++ [ time_gap | p in PassengerTrains ],
        [ 1 | f in FreightTrains ] ++ [ if PTT[p, r] > 0 then 1 else 0 endif | p in PassengerTrains ],
        [ freight_train_sx[f, r] | f in FreightTrains ] ++ [ PTT[p, r] > 0 | p in PassengerTrains ],
        1
    ));

constraint forall(r in RailLinks)(gecode_schedule_cumulative_optional(
        [ end_lb[f, r] | f in FreightTrains ] ++ [ PTT[p, r]+rail_links[r,LINKDURATION] | p in PassengerTrains ],
        [ time_gap | f in FreightTrains ] ++ [ time_gap | p in PassengerTrains ],
        [ 1 | f in FreightTrains ] ++ [ if PTT[p, r] > 0 then 1 else 0 endif | p in PassengerTrains ],
        [ freight_train_sx[f, r] | f in FreightTrains ] ++ [ PTT[p, r] > 0 | p in PassengerTrains ],   
        1
    ));

constraint forall (f in FreightTrains, r in RailLinks) (freight_train_sx[f,r] -> freight_train_ub[f, r] = freight_train_lb[f, r]) ;
constraint forall (f in FreightTrains, r in RailLinks) (freight_train_sx[f,r] -> end_ub[f, r] = end_lb[f, r]) ;
*/
%--------------Gecode - end--------------------

%==========Cumulative end=======================

%Objective function - Minimise the time of the last train in the network.
var 0..1440: Latest;
constraint forall(i in FreightTrains)(Latest >= freight_train_lb[i,freight_train_path[i,stopcount[i]]]) ;


solve ::
%--------------Search for Opturion-cpx: start--------------------

%limited_search(time_limit(600.0),
%cpx_warm_start(5000,
restart_luby(time_limit(0.5),
          seq_search([
                      int_search([freight_train_path[i,r]|r in MaxStops, i in FreightTrains], cpx_max_activity, indomain_min,complete),
%                    bool_search([order[i,j]| i,j in FreightTrains], cpx_max_activity, indomain_min,complete),                    
                    bool_search([freight_train_sx[i,r]| r in RailLinks, i in FreightTrains], cpx_max_activity, indomain_min,complete),
                    int_search([freight_train_lb[i,r]|r in RailLinks, i in FreightTrains], cpx_max_activity, indomain_min,complete),
                    int_search([end_lb[i,r]|r in RailLinks, i in FreightTrains], cpx_max_activity, indomain_min,complete),
                    default_search
]) %End seq_search
) %End restart_luby


%=====Other search labels for testing purpose==========                      
%                      int_search([stopcount[i]|i in FreightTrains], cpx_max_activity, indomain_min, complete),                                                                                      
%                      int_search([freight_train_ub[i,r]|r in RailLinks, i in FreightTrains], cpx_max_activity, indomain_min,complete),
%                      int_search([end_ub[i,r]|r in RailLinks, i in FreightTrains], cpx_max_activity, indomain_min,complete),
%=====Other search label: end==========================

%--------------Search for Opturion-cpx: End--------------------

%--------------Search for Gecode: start---------------
/*

          seq_search([
                    int_search([freight_train_path[i,r]|r in MaxStops, i in FreightTrains], first_fail, indomain_min,complete),
                    bool_search([order[i,j]| i,j in FreightTrains], first_fail, indomain_min,complete),                    
                    bool_search([freight_train_sx[i,r]| r in RailLinks, i in FreightTrains], first_fail, indomain_min,complete),
                    int_search([freight_train_lb[i,r]|r in RailLinks, i in FreightTrains], smallest, indomain_min,complete),
                    int_search([end_lb[i,r]|r in RailLinks, i in FreightTrains], smallest, indomain_min,complete)])
*/
%--------------Search for Gecode: End---------

minimize Latest; 

%Prints information on to the console
output 
       ["StopCount = \n"]++[ show_int(3,stopcount[i]) ++ " " ++
    if i == 0 then "\n" else "" endif
        | i in FreightTrains] ++ ["\n"] ++

["Freight Train Path (links) = \n"]++[ show_int(3,freight_train_path[i,j]) ++ " " ++
   if j == maxstops then "\n" else "" endif
        | i in FreightTrains,j in MaxStops ] ++ ["\n"] ++
/*
      ["Order = \n"]++[ show(order[i,j]) ++ "\t" ++
    if j == nfreight_trains then "\n" else "" endif
         | i,j in FreightTrains] ++ ["\n"] ++
*/
      ["Time Table = \n"]++[ show(if not fix(freight_train_sx[i,j]) then 0 else freight_train_lb[i,j] endif) ++ "\t" ++
    if j == nlinks then "\n" else "" endif
         | i in FreightTrains, j in RailLinks] ++ ["\n"] ++

["Last train arrival time is "] ++ [ show(Latest) ] ; 