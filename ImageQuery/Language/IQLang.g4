grammar IQLang;

@parser::header
{
	#pragma warning disable 3021
	using System;
	using System.Collections;
	using ImageQuery.Query;
	using ImageQuery.Query.Expressions;
	using ImageQuery.Query.Operators;
	using ImageQuery.Query.Statements;
	using ImageQuery.Query.Selection;
	using ImageQuery.Query.Value;
}

@parser::members
{
	protected const int EOF = Eof;
}

@lexer::header
{
	#pragma warning disable 3021
}

@lexer::members
{
	protected const int EOF = Eof;
	protected const int HIDDEN = Hidden;
}

/*
 * Parser Rules
 */

compileUnit returns [List<IQueryStatement> list]
	:	EOF {$list = new List<IQueryStatement>();}
	|	statements EOF {$list = $statements.list;}
	;

statements returns [List<IQueryStatement> list]
	:	{$list = new List<IQueryStatement>();}
	(
		statement {$list.Add($statement.stm);}
	)+
	;

statement returns [IQueryStatement stm]
	:	input_statement {$stm = $input_statement.stm;}
	|	output_statement {$stm = $output_statement.stm;}
	|	intermediate_statement {$stm = $intermediate_statement.stm;}
	|	define_number_statement {$stm = $define_number_statement.stm;}
	|	define_color_statement {$stm = $define_color_statement.stm;}
	|	define_number_parameter_statement {$stm = $define_number_parameter_statement.stm;}
	|	define_color_parameter_statement {$stm = $define_color_parameter_statement.stm;}
	|	define_iterator_parameter_statement {$stm = $define_iterator_parameter_statement.stm;}
	|	define_function_statement {$stm = $define_function_statement.stm;}
	|	apply_statement {$stm = $apply_statement.stm;}
	|	set_variable_statement {$stm = $set_variable_statement.stm;}
	|	function_statement {$stm = $function_statement.stm;}
	|	conditional_statement {$stm = $conditional_statement.stm;}
	|	while_statement {$stm = $while_statement.stm;}
	|	do_while_statement {$stm = $do_while_statement.stm;}
	;

input_statement returns [IQueryStatement stm]
	:	INPUT IDENT {$stm = new DefineInputStatement() {CanvasName = $IDENT.text};}
	;

output_statement returns [IQueryStatement stm]
	:	OUTPUT IDENT L_BRACKET w=expression COMMA h=expression R_BRACKET {$stm = new DefineOutputStatement() {CanvasName = $IDENT.text, W = $w.expr, H = $h.expr};}
	;

intermediate_statement returns [IQueryStatement stm]
	:	CANVAS IDENT L_BRACKET w=expression COMMA h=expression R_BRACKET {$stm = new DefineIntermediateStatement() {Name = $IDENT.text, W = $w.expr, H = $h.expr};}
	|	CANVAS IDENT EQUAL expression {$stm = new DefineIntermediateStatement() {Name = $IDENT.text, Value = $expression.expr};}
	;

define_number_statement returns [IQueryStatement stm]
	:	NUMBER_KW IDENT {$stm = new DefineNumberStatement() {Name = $IDENT.text};}
	|	NUMBER_KW IDENT EQUAL expression {$stm = new DefineNumberStatement() {Name = $IDENT.text, Value = $expression.expr};}
	;

define_color_statement returns [IQueryStatement stm]
	:	COLOR IDENT {$stm = new DefineColorStatement() {Name = $IDENT.text};}
	|	COLOR IDENT EQUAL expression {$stm = new DefineColorStatement() {Name = $IDENT.text, Value = $expression.expr};}
	;

define_number_parameter_statement returns [IQueryStatement stm]
	:	PARAM NUMBER_KW IDENT {$stm = new DefineNumberParameterStatement() {Name = $IDENT.text};}
	|	PARAM NUMBER_KW IDENT EQUAL expression {$stm = new DefineNumberParameterStatement() {Name = $IDENT.text, Value = $expression.expr};}
	;

define_color_parameter_statement returns [IQueryStatement stm]
	:	PARAM COLOR IDENT {$stm = new DefineColorParameterStatement() {Name = $IDENT.text};}
	|	PARAM COLOR IDENT EQUAL expression {$stm = new DefineColorParameterStatement() {Name = $IDENT.text, Value = $expression.expr};}
	;

define_iterator_parameter_statement returns [IQueryStatement stm]
	:	PARAM ITERATOR IDENT {$stm = new DefineIteratorParameterStatement() {Name = $IDENT.text};}
	|	PARAM ITERATOR IDENT EQUAL expression {$stm = new DefineIteratorParameterStatement() {Name = $IDENT.text, Value = $expression.expr};}
	;

define_function_statement returns [IQueryStatement stm]
	:	FUNC n=IDENT L_PAREN {var argNames = new List<string>();}
	(
		a=IDENT {argNames.Add($a.text);}
		(
			',' b=IDENT {argNames.Add($b.text);}
		)*
	)? R_PAREN {var df = new DefineFunctionStatement() {Name = $n.text, ArgumentNames = argNames.ToArray()};}
	(
		s=statements {df.Statements = $s.list.ToArray();}
	)?
		e=expression END {df.FinalExpression = $e.expr; $stm = df;}
	;

apply_statement returns [IQueryStatement stm]
	:	n=IDENT COLON selection {$stm = new ApplyStatement() {CanvasName = $n.text, Selection = $selection.select};}
	|	n=IDENT L_BRACKET x=expression R_BRACKET COLON selection {$stm = new ApplyStatement() {CanvasName = $n.text, Selection = $selection.select, XManipulation = $x.expr};}
	|	n=IDENT L_BRACKET x=expression COMMA y=expression R_BRACKET COLON selection {$stm = new ApplyStatement() {CanvasName = $n.text, Selection = $selection.select, XManipulation = $x.expr, YManipulation = $y.expr};}
	;

set_variable_statement returns [IQueryStatement stm]
	:	IDENT EQUAL expression {$stm = new SetVariableStatement() {Name = $IDENT.text, Value = $expression.expr};}
	;

function_statement returns [IQueryStatement stm]
	:	function {$stm = new ExpressionStatement() {Expression = $function.expr};}
	;

conditional_statement returns [IQueryStatement stm]
	:	IF t=expression THEN {var cond = new ConditionalStatement() {Condition = $expression.expr};}
		(
			ts=statements {cond.True = $ts.list.ToArray();}
		)?
		(
			{var elifList = new List<ConditionalStatement.ElseIfSection>();}
			(
				ELSEIF ei=expression THEN {var eis = new ConditionalStatement.ElseIfSection() {Condition = $ei.expr};}
				(
					eis=statements {eis.True = $eis.list.ToArray();}
				)?
				{elifList.Add(eis);}
			)+
			{cond.ElseIf = elifList.ToArray();}
		)?
		(
			ELSE
			(
				fs=statements {cond.False = $fs.list.ToArray();}
			)?
		)?
		END {$stm = cond;}
	;

while_statement returns [IQueryStatement stm]
	:	WHILE c=expression DO {var ws = new WhileStatement() {Condition = $c.expr};}
	(
		statements {ws.Statements = $statements.list.ToArray();}
	)?	END {$stm = ws;}
	;

do_while_statement returns [IQueryStatement stm]
	: DO {var ws = new DoWhileStatement();}
	(
		statements {ws.Statements = $statements.list.ToArray();}
	)? WHILE c=expression {ws.Condition = $c.expr; $stm = ws;}
	;

selection returns [ISelection select]
	:	SELECT m=expression FROM c=expression {$select = new BasicSelection() {Canvas = $c.expr, Manipulation = $m.expr};}
	|	SELECT m=expression FROM c=expression WHERE w=expression {$select = new BasicSelection() {Canvas = $c.expr, Manipulation = $m.expr, Where = $w.expr};}
	|	SELECT m=expression FROM c=expression WHERE w=expression ELSE e=expression {$select = new BasicSelection() {Canvas = $c.expr, Manipulation = $m.expr, Where = $w.expr, Else = $e.expr};}
	;

expression returns [IExpression expr]
	:	ternaryExpr {$expr = $ternaryExpr.expr;}
	;

ternaryExpr returns [IExpression expr]
	:	orExpr {$expr = $orExpr.expr;}
	|	c=orExpr QUESTION t=expression COLON f=expression {$expr = new TernaryExpression() {Condition = $c.expr, True = $t.expr, False = $f.expr};}
	;

orExpr returns [IExpression expr]
	:	andExpr {$expr = $andExpr.expr;}
	|	a=andExpr OR b=orExpr {$expr = new OrExpression() {Left = $a.expr, Right = $b.expr};}
	;

andExpr returns [IExpression expr]
	:	equalityExpr {$expr = $equalityExpr.expr;}
	|	a=equalityExpr AND b=andExpr {$expr = new AndExpression() {Left = $a.expr, Right = $b.expr};}
	;

equalityExpr returns [IExpression expr]
	:	addExpr {$expr = $addExpr.expr;}
	|	a=addExpr EQUAL EQUAL b=equalityExpr {$expr = new EqualityExpression() {Left = $a.expr, Right = $b.expr};}
	|	a=addExpr NOT EQUAL b=equalityExpr {$expr = new InequalityExpression() {Left = $a.expr, Right = $b.expr};}
	|	a=addExpr GREATER b=equalityExpr {$expr = new GreaterThanExpression() {Left = $a.expr, Right = $b.expr};}
	|	a=addExpr GREATER EQUAL b=equalityExpr {$expr = new GreaterThanOrEqualExpression() {Left = $a.expr, Right = $b.expr};}
	|	a=addExpr LESS b=equalityExpr {$expr = new LessThanExpression() {Left = $a.expr, Right = $b.expr};}
	|	a=addExpr LESS b=equalityExpr {$expr = new LessThanOrEqualExpression() {Left = $a.expr, Right = $b.expr};}
	;

addExpr returns [IExpression expr]
	:	mulExpr {$expr = $mulExpr.expr;}
	|	a=mulExpr PLUS b=addExpr {$expr = new AddExpression() {Left = $a.expr, Right = $b.expr};}
	|	a=mulExpr MINUS b=addExpr {$expr = new SubtractExpression() {Left = $a.expr, Right = $b.expr};}
	;

mulExpr returns [IExpression expr]
	:	notExpr {$expr = $notExpr.expr;}
	|	a=notExpr MULTIPLY b=mulExpr {$expr = new MultiplyExpression() {Left = $a.expr, Right = $b.expr};}
	|	a=notExpr DIVIDE b=mulExpr {$expr = new DivideExpression() {Left = $a.expr, Right = $b.expr};}
	|	a=notExpr MODULUS b=mulExpr {$expr = new ModulusExpression() {Left = $a.expr, Right = $b.expr};}
	;

notExpr returns [IExpression expr]
	:	NOT atom {$expr = new NotExpression() {Operand = $atom.expr};}
	|	atom {$expr = $atom.expr;}
	;

atom returns [IExpression expr]
	:	value {$expr = $value.expr;}
	|	function {$expr = $function.expr;}
	|	variable {$expr = $variable.expr;}
	|	L_PAREN expression R_PAREN {$expr = $expression.expr;}
	;

value returns [IExpression expr]
	:	number {$expr = $number.expr;}
	|	color {$expr = $color.expr;}
	|	B_TRUE {$expr = new BooleanExpression() {Value = true};}
	|	B_FALSE {$expr = new BooleanExpression() {Value = false};}
	|	iterator {$expr = $iterator.expr;}
	;

color returns [IExpression expr]
	:	L_BRACE
	(
		{$expr = new ColorExpression();}
	|	r=expression {$expr = new ColorExpression() {R = $r.expr};}
	|	r=expression COMMA g=expression {$expr = new ColorExpression() {R = $r.expr, G = $g.expr};}
	|	r=expression COMMA g=expression COMMA b=expression {$expr = new ColorExpression() {R = $r.expr, G = $g.expr, B = $b.expr};}
	|	r=expression COMMA g=expression COMMA b=expression COMMA a=expression {$expr = new ColorExpression() {R = $r.expr, G = $g.expr, B = $b.expr, A = $a.expr};}
	)
		R_BRACE
	;

number returns [IExpression expr]
	:	MINUS NUMBER {$expr = new NumberExpression() {Value = -Convert.ToSingle($NUMBER.text)};}
	|	NUMBER {$expr = new NumberExpression() {Value = Convert.ToSingle($NUMBER.text)};}
	;

function returns [IExpression expr]
	:	n=IDENT L_PAREN {var argList = new List<IExpression>();}
	(
		a=expression {argList.Add($a.expr);}
		(
			COMMA b=expression {argList.Add($b.expr);}
		)*
	)?
		R_PAREN {$expr = new FunctionCallExpression() {Name = $n.text, Arguments = argList.ToArray()};}
	;

variable returns [IExpression expr]
	:	IDENT L_BRACKET x=expression COMMA y=expression R_BRACKET DOT v=variable {$expr = new EnterSelectionEnvironmentExpression() {Name = $IDENT.text, X = $x.expr, Y = $y.expr, Subexpression = $v.expr};}
	|	IDENT DOT v=variable {$expr = new EnterEnvironmentExpression() {Name = $IDENT.text, Subexpression = $v.expr};}
	|	IDENT L_BRACKET x=expression R_BRACKET {$expr = new RetrieveIndexedVariableExpression() {Name = $IDENT.text, X = $x.expr};}
	|	IDENT L_BRACKET x=expression COMMA y=expression R_BRACKET {$expr = new RetrieveIndexedVariableExpression() {Name = $IDENT.text, X = $x.expr, Y = $y.expr};}
	|	IDENT {$expr = new RetrieveVariableExpression() {Name = $IDENT.text};}
	;

iterator returns [IExpression expr]
	: L_BRACKET w=expression COMMA h=expression R_BRACKET {$expr = new IteratorExpression() {Width = $w.expr, Height = $h.expr};}
	;

/*
 * Lexer Rules
 */

INPUT
	:	'input' | 'INPUT'
	;

OUTPUT
	:	'output' | 'OUTPUT'
	;

CANVAS
	:	'canvas' | 'CANVAS'
	;

ITERATOR
	:	'iterator' | 'ITERATOR'
	;

PARAM
	:	'param' | 'PARAM'
	;

COLOR
	:	'col' | 'COL'
	;

FUNC
	:	'func' | 'FUNC'
	;

NUMBER_KW
	:	'num' | 'NUM'
	;

APPLY
	:	'apply' | 'APPLY'
	;

SELECT
	:	'select' | 'SELECT'
	;

FROM
	:	'from' | 'FROM'
	;

WHERE
	:	'where' | 'WHERE'
	;

IF
	:	'if' | 'IF'
	;

THEN
	:	'then' | 'THEN'
	;

ELSEIF
	:	'elseif' | 'ELSEIF'
	;

ELSE
	:	'else' | 'ELSE'
	;

END
	:	'end' | 'END'
	;

WHILE
	:	'while' | 'WHILE'
	;

DO
	:	'do' | 'DO'
	;

B_TRUE
	:	'true' | 'TRUE'
	;

B_FALSE
	:	'false' | 'FALSE'
	;

AND
	:	'and' | 'AND'
	;

OR
	:	'or' | 'OR'
	;

L_BRACKET
	:	'['
	;

R_BRACKET
	:	']'
	;

L_BRACE
	:	'{'
	;

R_BRACE
	:	'}'
	;

L_PAREN
	:	'('
	;

R_PAREN
	:	')'
	;

QUESTION
	:	'?'
	;

COLON
	:	':'
	;

COMMA
	:	','
	;

DOT
	:	'.'
	;

EQUAL
	:	'='
	;

NOT
	:	'!'
	;

GREATER
	:	'>'
	;

LESS
	:	'<'
	;

PLUS
	:	'+'
	;

MINUS
	:	'-'
	;

MULTIPLY
	:	'*'
	;

DIVIDE
	:	'/'
	;

MODULUS
	:	'%'
	;

NUMBER
	:
	(
		[0-9]* '.' [0-9]+
	|	[0-9]+
	)
	;

IDENT
	:	[a-zA-Z_] [0-9a-zA-Z_]*
	;

WS
	:	[ \n\t\r] -> channel(HIDDEN)
	;

COMMENT
	:
	(
		'#' ~[\r\n]*
	|	'/*' .*? '*/'
	) -> channel(HIDDEN)
	;