@list command =
- clear :
  - undo
- resign
- draw
- clock :
  - time
- who
- next
- puzzle
- home

@ list piece =
- king
- queen
- bishop
- knight
- rook
- pawn

@ list file =
- a :
  - alpha
- b :
  - bravo
- c :
  - charlie
- d :
  - delta
- e :
  - echo
- f :
  - foxtrot
- g :
  - golf
- h :
  - hotel

@ list rank = 
- one
- two
- three
- four
- five
- six
- seven
- eight

#AlgebraicNotation_Section1
- move {@file} {@rank} to {@file} {@rank}
- {@file} {@rank} [moves] to {@file} {@rank}
- move {@piece} to {@file} {@rank}
- {@piece} [moves] to {@file} {@rank}
- {@piece} {@file} {@rank}
- king-side [castle]
- queen-side [castle]
- [promote] {@file} eight [to] {@piece}
- {@command}