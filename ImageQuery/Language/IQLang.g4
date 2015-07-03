grammar IQLang;

@parser::header
{
	#pragma warning disable 3021
	using System;
	using System.Collections;
	using ImageQuery.Query;
	using ImageQuery.Query.Expressions;
	using ImageQuery.Query.Statements;
	using ImageQuery.Query.Operators;
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
	|	apply_statement {$stm = $apply_statement.stm;}
	;

input_statement returns [IQueryStatement stm]
	:	INPUT IDENT {$stm = new DefineInputStatement() {CanvasName = $IDENT.text};}
	;

output_statement returns [IQueryStatement stm]
	:	OUTPUT IDENT L_BRACKET w=expression COMMA h=expression R_BRACKET {$stm = new DefineOutputStatement() {CanvasName = $IDENT.text, W = $w.expr, H = $h.expr};}
	;

intermediate_statement returns [IQueryStatement stm]
	:	CANVAS IDENT L_BRACKET w=expression COMMA h=expression R_BRACKET {$stm = new DefineIntermediateStatement() {CanvasName = $IDENT.text, W = $w.expr, H = $h.expr};}
	;

apply_statement returns [IQueryStatement stm]
	:	n=IDENT COLON selection {$stm = new ApplyStatement() {CanvasName = $n.text, Selection = $selection.select};}
	|	n=IDENT L_BRACKET x=expression R_BRACKET COLON selection {$stm = new ApplyStatement() {CanvasName = $n.text, Selection = $selection.select, XModulation = $x.expr};}
	|	n=IDENT L_BRACKET x=expression COMMA y=expression R_BRACKET COLON selection {$stm = new ApplyStatement() {CanvasName = $n.text, Selection = $selection.select, XModulation = $x.expr, YModulation = $y.expr};}
	;

selection returns [ISelection select]
	:	SELECT m=expression FROM n=IDENT {$select = new BasicSelection() {CanvasName = $n.text, Modulation = $m.expr};}
	|	SELECT m=expression FROM n=IDENT WHERE w=expression {$select = new BasicSelection() {CanvasName = $n.text, Modulation = $m.expr, Where = $w.expr};}
	;

expression returns [IExpression expr]
	:	orExpr {$expr = $orExpr.expr;}
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
	:	number {$expr = $number.expr;}
	|	color {$expr = $color.expr;}
	|	B_TRUE {$expr = new BooleanExpression() {Value = true};}
	|	B_FALSE {$expr = new BooleanExpression() {Value = false};}
	|	variable {$expr = $variable.expr;}
	|	L_PAREN expression R_PAREN {$expr = $expression.expr;}
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

variable returns [IExpression expr]
	:	IDENT DOT v=variable {$expr = new EnterEnvironmentExpression() {CanvasName = $IDENT.text, Subexpression = $v.expr};}
	|	IDENT L_BRACKET x=expression R_BRACKET {$expr = new RetrieveIndexedVariableExpression() {Name = $IDENT.text, X = $x.expr};}
	|	IDENT L_BRACKET x=expression COMMA y=expression R_BRACKET {$expr = new RetrieveIndexedVariableExpression() {Name = $IDENT.text, X = $x.expr, Y = $y.expr};}
	|	IDENT {$expr = new RetrieveVariableExpression() {Name = $IDENT.text};}
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

APPLY
	:	'apply' | 'APPLY'
	;

SELECT
	: 'select' | 'SELECT'
	;

FROM
	:	'from' | 'FROM'
	;

WHERE
	:	'where' | 'WHERE'
	;

TO
	:	'to' | 'TO'
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