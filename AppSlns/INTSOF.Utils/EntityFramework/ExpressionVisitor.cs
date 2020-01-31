#region Copyright
// **************************************************************************************************
// ExpressionVisitor.cs
//  
// 
//  Comments
// ---------------------------------------------------
// Initial Coding
// 
// 
//                          Copyright 2011 Intersoft Data Labs.
// 
//      All rights are reserved.  Reproduction or transmission in whole or in part, in any form or
//    by any means, electronic, mechanical or otherwise, is prohibited without the prior written
//    consent of the copyright owner.
// *************************************************************************************************
#endregion
namespace INTSOF.Utils.EntityFramework
{
   #region Using Directives

   using System;
   using System.Collections.Generic;
   using System.Collections.ObjectModel;
   using System.Linq.Expressions;

   #endregion

   /// <summary>
   ///   Entity Framework ExpressionVisitor for PredicateBuilder
   /// </summary>
   public abstract class ExpressionVisitor
   {
      #region Public Methods and Operators

      /// <summary>
      /// Visits the specified exp.
      /// </summary>
      /// <param name="expression">
      /// The exp. 
      /// </param>
      /// <returns>
      /// A Expression 
      /// </returns>
      /// <exception cref="Exception">
      /// </exception>
      public virtual Expression Visit(Expression expression)
      {
         if (expression.IsNull())
         {
            return null;
         }

         switch (expression.NodeType)
         {
            case ExpressionType.Negate:
            case ExpressionType.NegateChecked:
            case ExpressionType.Not:
            case ExpressionType.Convert:
            case ExpressionType.ConvertChecked:
            case ExpressionType.ArrayLength:
            case ExpressionType.Quote:
            case ExpressionType.TypeAs:
               return this.VisitUnary((UnaryExpression)expression);

            case ExpressionType.Add:
            case ExpressionType.AddChecked:
            case ExpressionType.Subtract:
            case ExpressionType.SubtractChecked:
            case ExpressionType.Multiply:
            case ExpressionType.MultiplyChecked:
            case ExpressionType.Divide:
            case ExpressionType.Modulo:
            case ExpressionType.And:
            case ExpressionType.AndAlso:
            case ExpressionType.Or:
            case ExpressionType.OrElse:
            case ExpressionType.LessThan:
            case ExpressionType.LessThanOrEqual:
            case ExpressionType.GreaterThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.Equal:
            case ExpressionType.NotEqual:
            case ExpressionType.Coalesce:
            case ExpressionType.ArrayIndex:
            case ExpressionType.RightShift:
            case ExpressionType.LeftShift:
            case ExpressionType.ExclusiveOr:
               return this.VisitBinary((BinaryExpression)expression);

            case ExpressionType.TypeIs:
               return this.VisitTypeIs((TypeBinaryExpression)expression);

            case ExpressionType.Conditional:
               return this.VisitConditional((ConditionalExpression)expression);

            case ExpressionType.Constant:
               return this.VisitConstant((ConstantExpression)expression);

            case ExpressionType.Parameter:
               return this.VisitParameter((ParameterExpression)expression);

            case ExpressionType.MemberAccess:
               return this.VisitMemberAccess((MemberExpression)expression);

            case ExpressionType.Call:
               return this.VisitMethodCall((MethodCallExpression)expression);

            case ExpressionType.Lambda:
               return this.VisitLambda((LambdaExpression)expression);

            case ExpressionType.New:
               return this.VisitNew((NewExpression)expression);

            case ExpressionType.NewArrayInit:
            case ExpressionType.NewArrayBounds:
               return this.VisitNewArray((NewArrayExpression)expression);

            case ExpressionType.Invoke:
               return this.VisitInvocation((InvocationExpression)expression);

            case ExpressionType.MemberInit:
               return this.VisitMemberInit((MemberInitExpression)expression);

            case ExpressionType.ListInit:
               return this.VisitListInit((ListInitExpression)expression);

            default:
               throw new Exception(string.Format("Expression type '{0}' is not supported.", expression.NodeType));
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Visits the binary.
      /// </summary>
      /// <param name="expression">
      /// The b. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitBinary(BinaryExpression expression)
      {
         Expression left = this.Visit(expression.Left);

         Expression right = this.Visit(expression.Right);

         Expression conversion = this.Visit(expression.Conversion);

         return left != expression.Left || right != expression.Right || conversion != expression.Conversion
                   ? (expression.NodeType == ExpressionType.Coalesce && expression.Conversion.IsNotNull()
                         ? Expression.Coalesce(left, right, conversion as LambdaExpression)
                         : Expression.MakeBinary(expression.NodeType, left, right, expression.IsLiftedToNull, expression.Method))
                   : expression;
      }

      /// <summary>
      /// Visits the binding.
      /// </summary>
      /// <param name="binding">
      /// The binding. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.MemberBinding 
      /// </returns>
      /// <exception cref="Exception">
      /// </exception>
      protected virtual MemberBinding VisitBinding(MemberBinding binding)
      {
         switch (binding.BindingType)
         {
            case MemberBindingType.Assignment:
               return this.VisitMemberAssignment((MemberAssignment)binding);

            case MemberBindingType.MemberBinding:
               return this.VisitMemberMemberBinding((MemberMemberBinding)binding);

            case MemberBindingType.ListBinding:
               return this.VisitMemberListBinding((MemberListBinding)binding);

            default:
               throw new Exception(string.Format("Binding type '{0}' is not supported.", binding.BindingType));
         }
      }

      /// <summary>
      /// Visits the binding list.
      /// </summary>
      /// <param name="originalBindings">
      /// The original. 
      /// </param>
      /// <returns>
      /// A System.Collections.Generic.IEnumerable &lt; System.Linq.Expressions.MemberBinding &gt; 
      /// </returns>
      protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> originalBindings)
      {
         List<MemberBinding> bindings = null;

         for (int i = 0, count = originalBindings.Count; i < count; i++)
         {
            MemberBinding binding = this.VisitBinding(originalBindings[i]);

            if (bindings.IsNotNull())
            {
               bindings.Add(binding);
            }
            else
            {
               if (binding != originalBindings[i])
               {
                  bindings = new List<MemberBinding>(count);

                  for (int j = 0; j < i; j++)
                  {
                     bindings.Add(originalBindings[j]);
                  }

                  bindings.Add(binding);
               }
            }
         }

         return bindings.IsNotNull()
                   ? (IEnumerable<MemberBinding>)bindings
                   : originalBindings;
      }

      /// <summary>
      /// Visits the conditional.
      /// </summary>
      /// <param name="expression">
      /// The c. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitConditional(ConditionalExpression expression)
      {
         Expression test = this.Visit(expression.Test);

         Expression ifTrue = this.Visit(expression.IfTrue);

         Expression ifFalse = this.Visit(expression.IfFalse);

         if (test != expression.Test || ifTrue != expression.IfTrue || ifFalse != expression.IfFalse)
         {
            return Expression.Condition(test, ifTrue, ifFalse);
         }

         return expression;
      }

      /// <summary>
      /// Visits the constant.
      /// </summary>
      /// <param name="expression">
      /// The c. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitConstant(ConstantExpression expression)
      {
         return expression;
      }

      /// <summary>
      /// Visits the element initializer.
      /// </summary>
      /// <param name="initializer">
      /// The initializer. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.ElementInit 
      /// </returns>
      protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
      {
         ReadOnlyCollection<Expression> arguments = this.VisitExpressionList(initializer.Arguments);

         return arguments != initializer.Arguments
                   ? Expression.ElementInit(initializer.AddMethod, arguments)
                   : initializer;
      }

      /// <summary>
      /// Visits the element initializer list.
      /// </summary>
      /// <param name="originalInitializers">
      /// The original. 
      /// </param>
      /// <returns>
      /// A System.Collections.Generic.IEnumerable &lt; System.Linq.Expressions.ElementInit &gt; 
      /// </returns>
      protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> originalInitializers)
      {
         List<ElementInit> initializers = null;

         for (int i = 0, count = originalInitializers.Count; i < count; i++)
         {
            ElementInit initializer = this.VisitElementInitializer(originalInitializers[i]);

            if (initializers.IsNotNull())
            {
               initializers.Add(initializer);
            }
            else if (initializer != originalInitializers[i])
            {
               initializers = new List<ElementInit>(count);

               for (int j = 0; j < i; j++)
               {
                  initializers.Add(originalInitializers[j]);
               }

               initializers.Add(initializer);
            }
         }

         return initializers.IsNotNull()
                   ? (IEnumerable<ElementInit>)initializers
                   : originalInitializers;
      }

      /// <summary>
      /// Visits the expression list.
      /// </summary>
      /// <param name="originalExpressions">
      /// The original. 
      /// </param>
      /// <returns>
      /// A System.Collections.ObjectModel.ReadOnlyCollection &lt; System.Linq.Expressions.Expression &gt; 
      /// </returns>
      protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> originalExpressions)
      {
         List<Expression> expressions = null;

         for (int i = 0, count = originalExpressions.Count; i < count; i++)
         {
            Expression expression = this.Visit(originalExpressions[i]);

            if (expressions.IsNotNull())
            {
               expressions.Add(expression);
            }
            else if (expression != originalExpressions[i])
            {
               expressions = new List<Expression>(count);
               for (int j = 0; j < i; j++)
               {
                  expressions.Add(originalExpressions[j]);
               }

               expressions.Add(expression);
            }
         }

         return expressions.IsNotNull()
                   ? expressions.AsReadOnly()
                   : originalExpressions;
      }

      /// <summary>
      /// Visits the invocation.
      /// </summary>
      /// <param name="invocationExpression">
      /// The iv. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitInvocation(InvocationExpression invocationExpression)
      {
         IEnumerable<Expression> arguments = this.VisitExpressionList(invocationExpression.Arguments);

         Expression expression = this.Visit(invocationExpression.Expression);

         if (arguments != invocationExpression.Arguments || expression != invocationExpression.Expression)
         {
            return Expression.Invoke(expression, arguments);
         }

         return invocationExpression;
      }

      /// <summary>
      /// Visits the lambda.
      /// </summary>
      /// <param name="lambda">
      /// The lambda. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitLambda(LambdaExpression lambda)
      {
         Expression body = this.Visit(lambda.Body);

         return body != lambda.Body
                   ? Expression.Lambda(lambda.Type, body, lambda.Parameters)
                   : lambda;
      }

      /// <summary>
      /// Visits the list init.
      /// </summary>
      /// <param name="expression">
      /// The init. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitListInit(ListInitExpression expression)
      {
         NewExpression newExpression = this.VisitNew(expression.NewExpression);

         IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(expression.Initializers);

         if (newExpression != expression.NewExpression || initializers != expression.Initializers)
         {
            return Expression.ListInit(newExpression, initializers);
         }

         return expression;
      }

      /// <summary>
      /// Visits the member access.
      /// </summary>
      /// <param name="memberExpression">
      /// The m. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitMemberAccess(MemberExpression memberExpression)
      {
         Expression expression = this.Visit(memberExpression.Expression);

         return expression != memberExpression.Expression
                   ? Expression.MakeMemberAccess(expression, memberExpression.Member)
                   : memberExpression;
      }

      /// <summary>
      /// Visits the member assignment.
      /// </summary>
      /// <param name="assignment">
      /// The assignment. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.MemberAssignment 
      /// </returns>
      protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
      {
         Expression expression = this.Visit(assignment.Expression);

         return expression != assignment.Expression
                   ? Expression.Bind(assignment.Member, expression)
                   : assignment;
      }

      /// <summary>
      /// Visits the member init.
      /// </summary>
      /// <param name="expression">
      /// The init. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitMemberInit(MemberInitExpression expression)
      {
         NewExpression newExpression = this.VisitNew(expression.NewExpression);

         IEnumerable<MemberBinding> bindings = this.VisitBindingList(expression.Bindings);

         return newExpression != expression.NewExpression || bindings != expression.Bindings
                   ? Expression.MemberInit(newExpression, bindings)
                   : expression;
      }

      /// <summary>
      /// Visits the member list binding.
      /// </summary>
      /// <param name="binding">
      /// The binding. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.MemberListBinding 
      /// </returns>
      protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
      {
         IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(binding.Initializers);

         return initializers != binding.Initializers
                   ? Expression.ListBind(binding.Member, initializers)
                   : binding;
      }

      /// <summary>
      /// Visits the member member binding.
      /// </summary>
      /// <param name="binding">
      /// The binding. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.MemberMemberBinding 
      /// </returns>
      protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
      {
         IEnumerable<MemberBinding> bindings = this.VisitBindingList(binding.Bindings);

         return bindings != binding.Bindings
                   ? Expression.MemberBind(binding.Member, bindings)
                   : binding;
      }

      /// <summary>
      /// Visits the method call.
      /// </summary>
      /// <param name="methodCallExpression">
      /// The m. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitMethodCall(MethodCallExpression methodCallExpression)
      {
         Expression expression = this.Visit(methodCallExpression.Object);

         IEnumerable<Expression> arguments = this.VisitExpressionList(methodCallExpression.Arguments);

         return expression != methodCallExpression.Object || arguments != methodCallExpression.Arguments
                   ? Expression.Call(expression, methodCallExpression.Method, arguments)
                   : methodCallExpression;
      }

      /// <summary>
      /// Visits the new.
      /// </summary>
      /// <param name="expression">
      /// The nex. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.NewExpression 
      /// </returns>
      protected virtual NewExpression VisitNew(NewExpression expression)
      {
         IEnumerable<Expression> arguments = this.VisitExpressionList(expression.Arguments);

         return arguments != expression.Arguments
                   ? (expression.Members.IsNotNull()
                         ? Expression.New(expression.Constructor, arguments, expression.Members)
                         : Expression.New(expression.Constructor, arguments))
                   : expression;
      }

      /// <summary>
      /// Visits the new array.
      /// </summary>
      /// <param name="expression">
      /// The na. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitNewArray(NewArrayExpression expression)
      {
         IEnumerable<Expression> expressions = this.VisitExpressionList(expression.Expressions);

         return expressions != expression.Expressions
                   ? (expression.NodeType == ExpressionType.NewArrayInit
                         ? Expression.NewArrayInit(expression.Type.GetElementType(), expressions)
                         : Expression.NewArrayBounds(expression.Type.GetElementType(), expressions))
                   : expression;
      }

      /// <summary>
      /// Visits the parameter.
      /// </summary>
      /// <param name="expression">
      /// The p. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitParameter(ParameterExpression expression)
      {
         return expression;
      }

      /// <summary>
      /// Visits the type is.
      /// </summary>
      /// <param name="binaryExpression">
      /// The b. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitTypeIs(TypeBinaryExpression binaryExpression)
      {
         Expression expression = this.Visit(binaryExpression.Expression);

         return expression != binaryExpression.Expression
                   ? Expression.TypeIs(expression, binaryExpression.TypeOperand)
                   : binaryExpression;
      }

      /// <summary>
      /// Visits the unary.
      /// </summary>
      /// <param name="unaryExpression">
      /// The u. 
      /// </param>
      /// <returns>
      /// A System.Linq.Expressions.Expression 
      /// </returns>
      protected virtual Expression VisitUnary(UnaryExpression unaryExpression)
      {
         Expression expression = this.Visit(unaryExpression.Operand);

         return expression != unaryExpression.Operand
                   ? Expression.MakeUnary(unaryExpression.NodeType, expression, unaryExpression.Type, unaryExpression.Method)
                   : unaryExpression;
      }

      #endregion
   }
}